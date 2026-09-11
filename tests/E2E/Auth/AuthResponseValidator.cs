using System.Collections;
using System.Reflection;
using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

internal static class AuthResponseValidator
{
    public static void Validate(
        string operation,
        Type? expectedType,
        ExecutionResult result,
        TestContext context)
    {
        // The generated WithHttpInfo methods of a void operation return ApiResponse<object>, so
        // typeof(object) is a void contract, not "anything goes".
        var voidContract = expectedType is null || expectedType == typeof(object);
        if (voidContract && result.Response is not null && !AnswersWithBody(operation))
            throw new InvalidOperationException(
                $"{operation} returned {result.Response.GetType().Name}; expected a void response.");

        if (!voidContract)
        {
            if (result.Response is null)
                throw new InvalidOperationException(
                    $"{operation} returned no response; expected {expectedType!.Name}.");
            if (!expectedType!.IsInstanceOfType(result.Response))
                throw new InvalidOperationException(
                    $"{operation} returned {result.Response.GetType().Name}; expected {expectedType.Name}.");
        }

        switch (operation)
        {
            case "CreateSaasUser":
            case "SignUp":
                RequireId(result.Response, operation);
                RequireString(result.Response, "Email", operation);
                if (operation == "SignUp")
                    RequireEquals(result.Response, "Email", context.GetRequired<string>("signup_email"), operation);
                break;
            case "CreateTenant":
            case "CreateTenantUser":
            case "CreateTenantInvitation":
                RequireId(result.Response, operation);
                if (operation == "CreateTenant")
                    RequireEquals(result.Response, "Name", context.GetRequired<string>("tenant_name"), operation);
                if (operation == "CreateTenantUser")
                    RequireEquals(result.Response, "Email", context.GetRequired<string>("expected_user_email"), operation);
                break;
            case "CreateEnv":
                RequireEquals(result.Response, "Id", context.GetRequired<int>("env_id"), operation);
                RequireString(result.Response, "Name", operation);
                RequireEquals(result.Response, "Name", context.GetRequired<string>("env_name"), operation);
                break;
            case "CreateRole":
                RequireString(result.Response, "RoleName", operation);
                RequireEquals(result.Response, "RoleName", context.GetRequired<string>("role_name"), operation);
                break;
            case "CreateUserAttribute":
                RequireEquals(result.Response, "AttributeName", context.GetRequired<string>("attribute_name"), operation);
                break;
            case "CreateSaasUserAttribute":
                RequireEquals(
                    result.Response,
                    "AttributeName",
                    context.GetRequired<string>("saas_user_attribute_name"),
                    operation);
                break;
            case "CreateTenantAttribute":
                RequireEquals(
                    result.Response,
                    "AttributeName",
                    context.GetRequired<string>("tenant_attribute_name"),
                    operation);
                break;
            case "GetSaasUser":
                RequireEquals(result.Response, "Id", context.GetRequired<string>("user_id"), operation);
                RequireEquals(result.Response, "Email", context.GetRequired<string>("expected_user_email"), operation);
                ValidateAttributes(result.Response, context, operation);
                break;
            case "GetTenant":
                RequireEquals(result.Response, "Id", context.GetRequired<string>("tenant_id"), operation);
                break;
            case "GetEnv":
                RequireEquals(result.Response, "Id", context.GetRequired<int>("env_id"), operation);
                RequireEquals(result.Response, "Name", context.GetRequired<string>("env_name"), operation);
                break;
            case "GetTenantUser":
                RequireEquals(result.Response, "Id", context.GetRequired<string>("tenant_user_id"), operation);
                RequireEquals(result.Response, "Email", context.GetRequired<string>("expected_user_email"), operation);
                ValidateAttributes(result.Response, context, operation);
                break;
            case "GetUserInfo":
                RequireString(result.Response, "Id", operation);
                RequireString(result.Response, "Email", operation);
                if (IsTrue(context, "cognito_user_ready") && IsTrue(context, "user_created"))
                    RequireEquals(result.Response, "Id", context.GetRequired<string>("user_id"), operation);
                break;
            case "GetUserInfoByEmail":
                RequireString(result.Response, "Id", operation);
                RequireString(result.Response, "Email", operation);
                if (IsTrue(context, "user_created"))
                {
                    RequireEquals(result.Response, "Id", context.GetRequired<string>("user_id"), operation);
                    RequireEquals(result.Response, "Email", context.GetRequired<string>("expected_user_email"), operation);
                }
                break;
            case "GetRoles":
                RequireCollectionMember(result.Response, "VarRoles", "RoleName", context, "role_name", operation);
                break;
            case "GetUserAttributes":
                RequireCollectionMember(result.Response, "VarUserAttributes", "AttributeName", context, "attribute_name", operation);
                break;
            case "GetTenantAttributes":
                RequireCollectionMember(result.Response, "VarTenantAttributes", "AttributeName", context, "tenant_attribute_name", operation);
                break;
            case "GetEnvs":
                if (IsTrue(context, "env_created"))
                    RequireCollectionMember(result.Response, "VarEnvs", "Id", context, "env_id", operation);
                break;
            case "GetTenants":
                if (IsTrue(context, "tenant_created"))
                    RequireCollectionMember(result.Response, "VarTenants", "Id", context, "tenant_id", operation);
                break;
            case "GetSaasUsers":
                if (IsTrue(context, "user_created"))
                    RequireCollectionMember(result.Response, "Users", "Id", context, "user_id", operation);
                break;
            case "GetTenantUsers":
            case "GetAllTenantUsers":
            case "GetAllTenantUser":
                if (IsTrue(context, "tenant_user_created"))
                    RequireCollectionMember(result.Response, "VarUsers", "Id", context, "tenant_user_id", operation);
                break;
            case "CreateAuthCredentials":
                RequireString(result.Response, "Code", operation);
                break;
            case "GetAuthCredentials":
                RequireString(result.Response, "AccessToken", operation);
                RequireString(result.Response, "IdToken", operation);
                break;
            case "CreateSecretCode":
                RequireString(result.Response, "SecretCode", operation);
                break;
        }
    }

    public static void ValidateRaw(
        string operation,
        CallStyle callStyle,
        ExecutionResult result)
    {
        if (callStyle is not (CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync))
            throw new InvalidOperationException($"Raw response story used unsupported call style {callStyle}.");
        if (result.Response is null)
            throw new InvalidOperationException($"{operation} returned no deserialized response.");
        if (result.Headers is null || result.Headers.Count == 0)
            throw new InvalidOperationException($"{operation} returned no HTTP headers.");
        var contentType = result.Headers
            .Where(pair => string.Equals(pair.Key, "Content-Type", StringComparison.OrdinalIgnoreCase))
            .SelectMany(pair => pair.Value)
            .FirstOrDefault();
        if (string.IsNullOrWhiteSpace(contentType) ||
            !contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"{operation} did not return a JSON Content-Type header.");
        if (string.IsNullOrWhiteSpace(result.Body))
            throw new InvalidOperationException($"{operation} returned an empty raw response body.");

        try
        {
            JToken.Parse(result.Body);
        }
        catch (Exception error)
        {
            throw new InvalidOperationException($"{operation} returned an invalid JSON response body.", error);
        }
    }

    private static void ValidateAttributes(object? response, TestContext context, string operation)
    {
        var saasUserAttribute = operation == "GetSaasUser" && IsTrue(context, "saas_user_attribute_created");
        var tenantUserAttribute = operation == "GetTenantUser" &&
                                  (IsTrue(context, "user_attribute_created") || IsTrue(context, "tenant_user_created"));
        if (!saasUserAttribute && !tenantUserAttribute) return;
        var attributes = GetProperty(response, "Attributes") as IDictionary;
        if (attributes is null)
            throw new InvalidOperationException($"{operation} returned no Attributes collection.");
        var expectedKey = context.GetRequired<string>(saasUserAttribute
            ? "saas_user_attribute_name"
            : "attribute_name");
        if (!attributes.Contains(expectedKey))
            throw new InvalidOperationException($"{operation} did not return attribute '{expectedKey}'.");
        var expectedValue = context.GetRequired<string>("expected_attribute_value");
        if (!string.Equals(attributes[expectedKey]?.ToString(), expectedValue, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"{operation} returned an unexpected value for attribute '{expectedKey}'.");
    }

    private static void RequireCollectionMember(
        object? response,
        string collectionProperty,
        string memberProperty,
        TestContext context,
        string variable,
        string operation)
    {
        var collection = GetProperty(response, collectionProperty) as IEnumerable;
        if (collection is null)
            throw new InvalidOperationException($"{operation} returned no {collectionProperty} collection.");
        var expected = context.Variables.TryGetValue(variable, out var value) ? value : null;
        if (!collection.Cast<object>().Any(item =>
                string.Equals(GetProperty(item, memberProperty)?.ToString(), expected?.ToString(), StringComparison.Ordinal)))
        {
            throw new InvalidOperationException(
                $"{operation} did not return the expected {memberProperty} '{expected}'.");
        }
    }

    /// <summary>
    /// Operations whose OpenAPI definition declares no response schema but that the API still
    /// answers with a JSON body. The generated method returns <c>ApiResponse&lt;object&gt;</c>, so the
    /// void-contract rule would reject the deserialized body of a successful call.
    /// </summary>
    private static bool AnswersWithBody(string operation) =>
        operation == "DeleteSaasUser";

    private static void RequireId(object? response, string operation) =>
        RequireString(response, "Id", operation);

    private static void RequireString(object? response, string property, string operation)
    {
        var value = GetProperty(response, property)?.ToString();
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"{operation} returned an empty {property}.");
    }

    private static void RequireEquals(object? response, string property, object expected, string operation)
    {
        var actual = GetProperty(response, property);
        if (!string.Equals(actual?.ToString(), expected.ToString(), StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"{operation} returned {property} '{actual}', expected '{expected}'.");
    }

    private static object? GetProperty(object? value, string propertyName) =>
        value?.GetType().GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?.GetValue(value);

    private static bool IsTrue(TestContext context, string key) =>
        context.Variables.TryGetValue(key, out var value) && value is true;
}
