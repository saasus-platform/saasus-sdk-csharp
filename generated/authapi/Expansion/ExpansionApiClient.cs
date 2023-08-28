using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace authapi.Client
{
    public partial class ApiClient
    {
        // Sigを作るためにSDKからSaaSPlatform発行するRestRequestが必要なため、InterceptRequestを使う
        partial void InterceptRequest(RestRequest request)
        {
            try
            {
                // this._baseUrlは、SaaSuaConfigurationのコンストラクタで設定される
                Task<string> sig = WithSaasusSigV1(request, this._baseUrl);
                request.AddHeader("Authorization", sig.Result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed WithSaasusSigV1" + ex.Message);
                throw;
            }
        }

        private static async Task<string> WithSaasusSigV1(RestRequest request, string basePath)
        {
            string secret = Environment.GetEnvironmentVariable("SAASUS_SECRET_KEY") ?? "";
            string apiKey = Environment.GetEnvironmentVariable("SAASUS_API_KEY") ?? "";
            string saasID = Environment.GetEnvironmentVariable("SAASUS_SAAS_ID") ?? "";
            string literal = "SAASUSSIGV1";

            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(saasID))
            {
                throw new Exception("Invalid request: SAASUS_SECRET_KEY, SAASUS_API_KEY, SAASUS_SAAS_ID are required");
            }

            HMACSHA256 signatureHmac = new HMACSHA256(Encoding.ASCII.GetBytes(secret));

            string now = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            string method = request.Method.ToString().ToUpper();


            // この時点ではpathパラメータがバインドされてないため一時的にバインド
            string applyResource = request.Resource;
            foreach (var param in request.Parameters.Where(p => p.Type == ParameterType.UrlSegment))
            {
                if (applyResource.Contains("{" + param.Name + "}"))
                {
                    applyResource = applyResource.Replace("{" + param.Name + "}", param.Value.ToString());
                }
            }

            // Extract query parameters from the request
            string queryString = string.Join("&", request.Parameters
                .Where(p => p.Type == ParameterType.QueryString)
                .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.Value.ToString())}"));

            string[] splitBaseUri = basePath.Split(new string[] { "//" }, StringSplitOptions.None);

            // Concatenate to give the host + path + query string
            string fullResourceUri = queryString != string.Empty
                ? $"{splitBaseUri[1]}{applyResource}?{queryString}"
                : $"{splitBaseUri[1]}{applyResource}";

            string body = "";
            var bodyParameter = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            if (bodyParameter != null)
            {
                body = bodyParameter.Value.ToString();
            }

            var message = string.Concat(now, apiKey, method, fullResourceUri, body);
            signatureHmac.ComputeHash(Encoding.ASCII.GetBytes(message));

            string authorization = $"{literal} Sig={BitConverter.ToString(signatureHmac.Hash).Replace("-", "").ToLower()}, SaaSID={saasID}, APIKey={apiKey}";

            return authorization;
        }
    }
}
