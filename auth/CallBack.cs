using authapi.Api;
using authapi.Client;
using authapi.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace saasus_sdk_csharp.auth
{
    public class CallBack
    {
        public saasus_sdk_csharp.auth.AuthConfiguration AuthConfig { get; }

        public CallBack(saasus_sdk_csharp.auth.AuthConfiguration config)
        {
            AuthConfig = config;
        }

        public string GetIdToken(string codeParam)
        {

            string idToken = "";

            if (string.IsNullOrEmpty(codeParam))
            {
                throw new Exception("Query parameter cod not set");
            }

            var credentialApi = new CredentialApi(this.AuthConfig.GetAuthConfiguration());
            try
            {
                Credentials credentials = credentialApi.GetAuthCredentials(code: codeParam, authFlow: "tempCodeAuth");
                idToken = credentials.IdToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed GetAuthCredentials" + ex.Message);
                throw;
            }
            return idToken;
        }

        public UserInfo GetUserInfo(string iDtoken)
        {
            var userInfoApi = new UserInfoApi(this.AuthConfig.GetAuthConfiguration());

            try
            {
                UserInfo userInfo = userInfoApi.GetUserInfo(iDtoken);
                return userInfo;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed GetUserInfo" + ex.Message);
                throw;
            }
        }
    }
}
