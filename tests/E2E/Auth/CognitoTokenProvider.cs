using System.Net.Http.Headers;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

internal sealed record CognitoTokens(
    string AccessToken,
    string IdToken,
    string RefreshToken);

internal sealed class CognitoTokenProvider : IDisposable
{
    private const string Service = "cognito-idp";
    private const string TargetPrefix = "AWSCognitoIdentityProviderService.";

    private readonly Config _config;
    private readonly HttpClient _httpClient;
    private readonly List<string> _provisionedUsernames = new();

    private CognitoTokenProvider(Config config)
    {
        _config = config;
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public CognitoTokens ConfiguredUserTokens { get; private set; } = null!;

    /// <summary>The provider owns its <see cref="HttpClient"/>, so callers must dispose it.</summary>
    public void Dispose() => _httpClient.Dispose();

    public bool CanProvisionUsers =>
        !string.IsNullOrWhiteSpace(_config.AwsAccessKeyId) &&
        !string.IsNullOrWhiteSpace(_config.AwsSecretAccessKey);

    public static async Task<(CognitoTokenProvider? Provider, string? Reason)> TryCreateAsync(
        Config config,
        CancellationToken cancellationToken)
    {
        var missing = new[]
        {
            ("E2E_COGNITO_USER_POOL_ID", config.CognitoUserPoolId),
            ("E2E_COGNITO_CLIENT_ID", config.CognitoClientId),
            ("E2E_COGNITO_USERNAME", config.CognitoUsername),
            ("E2E_COGNITO_PASSWORD", config.CognitoPassword)
        }
        .Where(pair => string.IsNullOrWhiteSpace(pair.Item2))
        .Select(pair => pair.Item1)
        .ToArray();

        if (missing.Length > 0)
            return (null, $"Missing Cognito environment variables: {string.Join(", ", missing)}.");

        var provider = new CognitoTokenProvider(config);
        try
        {
            provider.ConfiguredUserTokens = await provider.AuthenticateAsync(
                config.CognitoUsername,
                config.CognitoPassword,
                cancellationToken).ConfigureAwait(false);
            return (provider, null);
        }
        catch (Exception error)
        {
            provider._httpClient.Dispose();
            return (null, $"Cognito authentication was unavailable: {error.Message}");
        }
    }

    public async Task<CognitoTokens> EnsureUserTokensAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        try
        {
            return await AuthenticateAsync(email, password, cancellationToken).ConfigureAwait(false);
        }
        catch (CognitoApiException) when (CanProvisionUsers)
        {
            var stableUsername = StableUsername(email);
            var username = await FindUsernameAsync(email, cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(username))
            {
                username = stableUsername;
                try
                {
                    await CallAsync(
                        "AdminCreateUser",
                        new
                        {
                            UserPoolId = _config.CognitoUserPoolId,
                            Username = username,
                            UserAttributes = new[]
                            {
                                new { Name = "email", Value = email },
                                new { Name = "email_verified", Value = "true" }
                            },
                            MessageAction = "SUPPRESS"
                        },
                        sign: true,
                        cancellationToken).ConfigureAwait(false);
                    Track(username);
                }
                catch (CognitoApiException error) when (
                    error.ErrorType is "UsernameExistsException" or "AliasExistsException")
                {
                    // The user may have been created by a concurrent E2E story.
                }
            }

            // Only the accounts this suite provisions carry the derived username. Resetting the
            // password of any other account would change a credential this run neither created nor
            // can restore, so such an account is reported instead of modified.
            if (!string.Equals(username, stableUsername, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"A Cognito user for the story email already exists as '{username}' and was not " +
                    "provisioned by this run, so its password was left unchanged.");

            await CallAsync(
                "AdminSetUserPassword",
                new
                {
                    UserPoolId = _config.CognitoUserPoolId,
                    Username = username,
                    Password = password,
                    Permanent = true
                },
                sign: true,
                cancellationToken).ConfigureAwait(false);

            return await AuthenticateAsync(email, password, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Deletes the Cognito users this provider created, so a story does not leave accounts behind
    /// in the shared user pool. Users that already existed are left untouched.
    /// </summary>
    public async Task DeleteProvisionedUsersAsync(CancellationToken cancellationToken)
    {
        string[] usernames;
        lock (_provisionedUsernames)
        {
            usernames = _provisionedUsernames.ToArray();
        }

        var failures = new List<Exception>();
        foreach (var username in usernames)
        {
            var deleted = false;
            try
            {
                await CallAsync(
                    "AdminDeleteUser",
                    new
                    {
                        UserPoolId = _config.CognitoUserPoolId,
                        Username = username
                    },
                    sign: true,
                    cancellationToken).ConfigureAwait(false);
                deleted = true;
            }
            catch (CognitoApiException error) when (error.ErrorType == "UserNotFoundException")
            {
                // The SaaSus user deletion already removed the account.
                deleted = true;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception error)
            {
                failures.Add(error);
            }

            // The username is forgotten only once the account is really gone, so a transient
            // failure can still be retried by a later cleanup instead of leaking the account.
            if (deleted)
            {
                lock (_provisionedUsernames)
                {
                    _provisionedUsernames.Remove(username);
                }
            }
        }

        if (failures.Count == 1) throw failures[0];
        if (failures.Count > 1)
            throw new AggregateException("Cognito user cleanup did not complete.", failures);
    }

    public async Task<string> AssociateSoftwareTokenAsync(
        string accessToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new InvalidOperationException("A Cognito access token is required for MFA setup.");

        var response = await CallAsync(
            "AssociateSoftwareToken",
            new { AccessToken = accessToken },
            sign: CanProvisionUsers,
            cancellationToken).ConfigureAwait(false);
        var secret = response.Value<string>("SecretCode");
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("Cognito AssociateSoftwareToken returned no SecretCode.");
        return secret;
    }

    public static string GenerateCurrentTotp(string secret)
    {
        var key = DecodeBase32(secret);
        var counter = BitConverter.GetBytes(DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30);
        if (BitConverter.IsLittleEndian) Array.Reverse(counter);

        using var hmac = new HMACSHA1(key);
        var hash = hmac.ComputeHash(counter);
        var offset = hash[^1] & 0x0f;
        var binary = ((hash[offset] & 0x7f) << 24) |
                     (hash[offset + 1] << 16) |
                     (hash[offset + 2] << 8) |
                     hash[offset + 3];
        return (binary % 1_000_000).ToString("D6", CultureInfo.InvariantCulture);
    }

    private async Task<CognitoTokens> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken)
    {
        JObject response;
        var userPayload = new
        {
            AuthFlow = "USER_PASSWORD_AUTH",
            ClientId = _config.CognitoClientId,
            AuthParameters = new { USERNAME = username, PASSWORD = password }
        };

        try
        {
            response = await CallAsync(
                "InitiateAuth",
                userPayload,
                sign: false,
                cancellationToken).ConfigureAwait(false);
        }
        catch (CognitoApiException) when (CanProvisionUsers)
        {
            response = await CallAsync(
                "AdminInitiateAuth",
                new
                {
                    UserPoolId = _config.CognitoUserPoolId,
                    ClientId = _config.CognitoClientId,
                    AuthFlow = "ADMIN_USER_PASSWORD_AUTH",
                    AuthParameters = new { USERNAME = username, PASSWORD = password }
                },
                sign: true,
                cancellationToken).ConfigureAwait(false);
        }

        if (string.Equals(response.Value<string>("ChallengeName"), "NEW_PASSWORD_REQUIRED", StringComparison.Ordinal))
        {
            response = await CallAsync(
                "RespondToAuthChallenge",
                new
                {
                    ChallengeName = "NEW_PASSWORD_REQUIRED",
                    ClientId = _config.CognitoClientId,
                    ChallengeResponses = new { USERNAME = username, NEW_PASSWORD = password },
                    Session = response.Value<string>("Session")
                },
                sign: CanProvisionUsers,
                cancellationToken).ConfigureAwait(false);
        }

        var authentication = response["AuthenticationResult"] as JObject
            ?? throw new InvalidOperationException("Cognito AuthenticationResult is empty.");
        var tokens = new CognitoTokens(
            authentication.Value<string>("AccessToken") ?? string.Empty,
            authentication.Value<string>("IdToken") ?? string.Empty,
            authentication.Value<string>("RefreshToken") ?? string.Empty);
        if (string.IsNullOrWhiteSpace(tokens.AccessToken) || string.IsNullOrWhiteSpace(tokens.IdToken))
            throw new InvalidOperationException("Cognito AuthenticationResult did not contain the required tokens.");
        return tokens;
    }

    private async Task<string> FindUsernameAsync(string email, CancellationToken cancellationToken)
    {
        var response = await CallAsync(
            "ListUsers",
            new
            {
                UserPoolId = _config.CognitoUserPoolId,
                Filter = $"email = \"{email.Replace("\"", "\\\"", StringComparison.Ordinal)}\"",
                Limit = 1
            },
            sign: true,
            cancellationToken).ConfigureAwait(false);
        return response["Users"]?.FirstOrDefault()?.Value<string>("Username") ?? string.Empty;
    }

    private async Task<JObject> CallAsync(
        string operation,
        object payload,
        bool sign,
        CancellationToken cancellationToken)
    {
        var body = JsonConvert.SerializeObject(payload);
        var uri = new Uri(Endpoint(), UriKind.Absolute);
        var timestamp = DateTime.UtcNow;
        // SigV4 timestamps must be Gregorian with ASCII digits regardless of the current culture.
        var amzDate = timestamp.ToString("yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
        var date = timestamp.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var host = uri.IsDefaultPort ? uri.Host : $"{uri.Host}:{uri.Port}";
        var headers = new SortedDictionary<string, string>(StringComparer.Ordinal)
        {
            ["content-type"] = "application/x-amz-json-1.1",
            ["host"] = host,
            ["x-amz-date"] = amzDate,
            ["x-amz-target"] = TargetPrefix + operation
        };
        if (!string.IsNullOrWhiteSpace(_config.AwsSessionToken))
            headers["x-amz-security-token"] = _config.AwsSessionToken;

        using var request = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new StringContent(body, Encoding.UTF8)
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue(headers["content-type"]);
        request.Headers.Host = host;
        request.Headers.TryAddWithoutValidation("X-Amz-Date", amzDate);
        request.Headers.TryAddWithoutValidation("X-Amz-Target", TargetPrefix + operation);
        if (headers.TryGetValue("x-amz-security-token", out var sessionToken))
            request.Headers.TryAddWithoutValidation("X-Amz-Security-Token", sessionToken);
        if (sign)
        {
            if (!CanProvisionUsers)
                throw new InvalidOperationException(
                    "AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY are required for signed Cognito operations.");
            request.Headers.TryAddWithoutValidation(
                "Authorization",
                AuthorizationHeader(body, headers, date, CanonicalPath(uri)));
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        JObject decoded;
        try
        {
            decoded = string.IsNullOrWhiteSpace(responseBody)
                ? new JObject()
                : JObject.Parse(responseBody);
        }
        catch (JsonException error)
        {
            throw new InvalidOperationException(
                $"Cognito {operation} returned a non-JSON response (HTTP {(int)response.StatusCode}).", error);
        }

        if (!response.IsSuccessStatusCode)
        {
            var rawType = decoded.Value<string>("__type") ?? "CognitoError";
            var errorType = rawType[(rawType.LastIndexOf('#') + 1)..];
            throw new CognitoApiException(
                operation,
                errorType,
                decoded.Value<string>("message") ?? responseBody,
                (int)response.StatusCode);
        }

        return decoded;
    }

    private string AuthorizationHeader(
        string body,
        SortedDictionary<string, string> headers,
        string date,
        string canonicalPath)
    {
        var canonicalHeaders = string.Join(
            string.Empty,
            headers.Select(pair => $"{pair.Key}:{pair.Value.Trim()}\n"));
        var signedHeaders = string.Join(";", headers.Keys);
        var canonicalRequest = string.Join(
            "\n",
            "POST",
            canonicalPath,
            string.Empty,
            canonicalHeaders,
            signedHeaders,
            Sha256(body));
        var scope = $"{date}/{_config.CognitoRegion}/{Service}/aws4_request";
        var stringToSign = string.Join(
            "\n",
            "AWS4-HMAC-SHA256",
            headers["x-amz-date"],
            scope,
            Sha256(canonicalRequest));
        var dateKey = Hmac("AWS4" + _config.AwsSecretAccessKey, date);
        var regionKey = Hmac(dateKey, _config.CognitoRegion);
        var serviceKey = Hmac(regionKey, Service);
        var signingKey = Hmac(serviceKey, "aws4_request");
        var signature = Convert.ToHexString(Hmac(signingKey, stringToSign)).ToLowerInvariant();
        return $"AWS4-HMAC-SHA256 Credential={_config.AwsAccessKeyId}/{scope}, " +
               $"SignedHeaders={signedHeaders}, Signature={signature}";
    }

    private string Endpoint() => string.IsNullOrWhiteSpace(_config.CognitoEndpoint)
        ? $"https://cognito-idp.{_config.CognitoRegion}.amazonaws.com/"
        : _config.CognitoEndpoint.TrimEnd('/') + "/";

    /// <summary>
    /// The path SigV4 has to sign. A configured endpoint may be mounted under a path, and signing
    /// "/" instead would make the proxy reject the request with a signature mismatch.
    /// </summary>
    internal static string CanonicalPath(Uri uri) =>
        string.IsNullOrEmpty(uri.AbsolutePath) ? "/" : uri.AbsolutePath;

    private void Track(string username)
    {
        lock (_provisionedUsernames)
            _provisionedUsernames.Add(username);
    }

    private static byte[] Hmac(string key, string value) =>
        Hmac(Encoding.UTF8.GetBytes(key), value);

    private static byte[] Hmac(byte[] key, string value)
    {
        using var hmac = new HMACSHA256(key);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
    }

    private static string Sha256(string value)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
    }

    private static byte[] DecodeBase32(string value)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var bits = new StringBuilder();
        foreach (var character in value.TrimEnd('=').ToUpperInvariant())
        {
            var index = alphabet.IndexOf(character);
            if (index < 0) throw new FormatException("Invalid Base32 software token secret.");
            bits.Append(Convert.ToString(index, 2).PadLeft(5, '0'));
        }

        var bytes = new List<byte>();
        for (var offset = 0; offset + 8 <= bits.Length; offset += 8)
            bytes.Add(Convert.ToByte(bits.ToString(offset, 8), 2));
        return bytes.ToArray();
    }

    private static string StableUsername(string email)
    {
        using var sha = SHA1.Create();
        return "e2e-" + Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(email.ToLowerInvariant())))
            .ToLowerInvariant();
    }

    private sealed class CognitoApiException : Exception
    {
        public CognitoApiException(string operation, string errorType, string message, int statusCode)
            : base($"Cognito {operation} failed with HTTP {statusCode}: {message}")
        {
            ErrorType = errorType;
        }

        public string ErrorType { get; }
    }
}
