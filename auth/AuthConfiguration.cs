using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using authapi.Model;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using authapi.Client;

namespace saasus_sdk_csharp.auth
{
    public class AuthConfiguration
    {
        public Configuration AuthConfig { get; set; }

        public AuthConfiguration(string secretKey, string apiKey, string saasIdKey, string baseAuthURL)
        {

            Environment.SetEnvironmentVariable("BACE_AUTH_URL", baseAuthURL);
            Environment.SetEnvironmentVariable("SAASUS_SECRET_KEY", secretKey);
            Environment.SetEnvironmentVariable("SAASUS_API_KEY", apiKey);
            Environment.SetEnvironmentVariable("SAASUS_SAAS_ID", saasIdKey);

            Configuration config = new Configuration();
            config.BasePath = baseAuthURL;
            AuthConfig = config;
        }

        public Configuration GetAuthConfiguration()
        {
            return AuthConfig;
        }
    }
}
