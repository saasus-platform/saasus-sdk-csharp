using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace SaaSus
{
    internal sealed class SdkUserAgent : IAuthenticator
    {
        internal static readonly SdkUserAgent Instance = new SdkUserAgent();

        private const string Header = "User-Agent";
        private const string Product = "SaaSus-SDK-csharp/" + SdkVersion.Value;

        private SdkUserAgent()
        {
        }

        public ValueTask Authenticate(IRestClient client, RestRequest request)
        {
            // This RestSharp hook exposes the client options before HttpClient copies its default headers.
            var requestUserAgent = request.Parameters.GetParameters(ParameterType.HttpHeader)
                .LastOrDefault(parameter => string.Equals(parameter.Name, Header, StringComparison.OrdinalIgnoreCase));
            var userAgent = requestUserAgent?.Value?.ToString() ?? client.Options.UserAgent ?? string.Empty;

            if (!userAgent.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Contains(Product))
            {
                userAgent = (userAgent + " " + Product).Trim();
            }
            request.AddOrUpdateHeader(Header, userAgent);

            return default;
        }
    }
}
