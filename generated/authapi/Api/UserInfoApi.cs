/*
 * SaaSus Auth API Schema
 *
 * Schema
 *
 * The version of the OpenAPI document: 1.0.0
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using authapi.Client;
using authapi.Model;

namespace authapi.Api
{

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUserInfoApiSync : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// Get User Info
        /// </summary>
        /// <remarks>
        /// User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserInfo</returns>
        UserInfo GetUserInfo(string token, int operationIndex = 0);

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <remarks>
        /// User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserInfo</returns>
        ApiResponse<UserInfo> GetUserInfoWithHttpInfo(string token, int operationIndex = 0);
        /// <summary>
        /// Get User Info by Email
        /// </summary>
        /// <remarks>
        /// Get user information by email address. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserInfo</returns>
        UserInfo GetUserInfoByEmail(string email, int operationIndex = 0);

        /// <summary>
        /// Get User Info by Email
        /// </summary>
        /// <remarks>
        /// Get user information by email address. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserInfo</returns>
        ApiResponse<UserInfo> GetUserInfoByEmailWithHttpInfo(string email, int operationIndex = 0);
        #endregion Synchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUserInfoApiAsync : IApiAccessor
    {
        #region Asynchronous Operations
        /// <summary>
        /// Get User Info
        /// </summary>
        /// <remarks>
        /// User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserInfo</returns>
        System.Threading.Tasks.Task<UserInfo> GetUserInfoAsync(string token, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get User Info
        /// </summary>
        /// <remarks>
        /// User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserInfo)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserInfo>> GetUserInfoWithHttpInfoAsync(string token, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        /// <summary>
        /// Get User Info by Email
        /// </summary>
        /// <remarks>
        /// Get user information by email address. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserInfo</returns>
        System.Threading.Tasks.Task<UserInfo> GetUserInfoByEmailAsync(string email, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));

        /// <summary>
        /// Get User Info by Email
        /// </summary>
        /// <remarks>
        /// Get user information by email address. 
        /// </remarks>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserInfo)</returns>
        System.Threading.Tasks.Task<ApiResponse<UserInfo>> GetUserInfoByEmailWithHttpInfoAsync(string email, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IUserInfoApi : IUserInfoApiSync, IUserInfoApiAsync
    {

    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class UserInfoApi : IUserInfoApi
    {
        private authapi.Client.ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UserInfoApi() : this((string)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UserInfoApi(string basePath)
        {
            this.Configuration = authapi.Client.Configuration.MergeConfigurations(
                authapi.Client.GlobalConfiguration.Instance,
                new authapi.Client.Configuration { BasePath = basePath }
            );
            this.Client = new authapi.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new authapi.Client.ApiClient(this.Configuration.BasePath);
            this.ExceptionFactory = authapi.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public UserInfoApi(authapi.Client.Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Configuration = authapi.Client.Configuration.MergeConfigurations(
                authapi.Client.GlobalConfiguration.Instance,
                configuration
            );
            this.Client = new authapi.Client.ApiClient(this.Configuration.BasePath);
            this.AsynchronousClient = new authapi.Client.ApiClient(this.Configuration.BasePath);
            ExceptionFactory = authapi.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoApi"/> class
        /// using a Configuration object and client instance.
        /// </summary>
        /// <param name="client">The client interface for synchronous API access.</param>
        /// <param name="asyncClient">The client interface for asynchronous API access.</param>
        /// <param name="configuration">The configuration object.</param>
        public UserInfoApi(authapi.Client.ISynchronousClient client, authapi.Client.IAsynchronousClient asyncClient, authapi.Client.IReadableConfiguration configuration)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (asyncClient == null) throw new ArgumentNullException("asyncClient");
            if (configuration == null) throw new ArgumentNullException("configuration");

            this.Client = client;
            this.AsynchronousClient = asyncClient;
            this.Configuration = configuration;
            this.ExceptionFactory = authapi.Client.Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// The client for accessing this underlying API asynchronously.
        /// </summary>
        public authapi.Client.IAsynchronousClient AsynchronousClient { get; set; }

        /// <summary>
        /// The client for accessing this underlying API synchronously.
        /// </summary>
        public authapi.Client.ISynchronousClient Client { get; set; }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.BasePath;
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public authapi.Client.IReadableConfiguration Configuration { get; set; }

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public authapi.Client.ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Get User Info User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserInfo</returns>
        public UserInfo GetUserInfo(string token, int operationIndex = 0)
        {
            authapi.Client.ApiResponse<UserInfo> localVarResponse = GetUserInfoWithHttpInfo(token);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get User Info User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserInfo</returns>
        public authapi.Client.ApiResponse<UserInfo> GetUserInfoWithHttpInfo(string token, int operationIndex = 0)
        {
            // verify the required parameter 'token' is set
            if (token == null)
            {
                throw new authapi.Client.ApiException(400, "Missing required parameter 'token' when calling UserInfoApi->GetUserInfo");
            }

            authapi.Client.RequestOptions localVarRequestOptions = new authapi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = authapi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = authapi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.QueryParameters.Add(authapi.Client.ClientUtils.ParameterToMultiMap("", "token", token));

            localVarRequestOptions.Operation = "UserInfoApi.GetUserInfo";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<UserInfo>("/userinfo", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserInfo", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get User Info User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserInfo</returns>
        public async System.Threading.Tasks.Task<UserInfo> GetUserInfoAsync(string token, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            authapi.Client.ApiResponse<UserInfo> localVarResponse = await GetUserInfoWithHttpInfoAsync(token, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get User Info User information is obtained based on the ID token of the SaaS user (registered user). The ID token is passed to the Callback URL during login from the SaaSus Platform generated login screen. User information can be obtained from calling this API with an ID token from the URL on the server side. Since the acquired tenant, role (role), price plan, etc. are included, it is possible to implement authorization based on it. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="token">ID Token</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserInfo)</returns>
        public async System.Threading.Tasks.Task<authapi.Client.ApiResponse<UserInfo>> GetUserInfoWithHttpInfoAsync(string token, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'token' is set
            if (token == null)
            {
                throw new authapi.Client.ApiException(400, "Missing required parameter 'token' when calling UserInfoApi->GetUserInfo");
            }


            authapi.Client.RequestOptions localVarRequestOptions = new authapi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = authapi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = authapi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.QueryParameters.Add(authapi.Client.ClientUtils.ParameterToMultiMap("", "token", token));

            localVarRequestOptions.Operation = "UserInfoApi.GetUserInfo";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<UserInfo>("/userinfo", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserInfo", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get User Info by Email Get user information by email address. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>UserInfo</returns>
        public UserInfo GetUserInfoByEmail(string email, int operationIndex = 0)
        {
            authapi.Client.ApiResponse<UserInfo> localVarResponse = GetUserInfoByEmailWithHttpInfo(email);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get User Info by Email Get user information by email address. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <returns>ApiResponse of UserInfo</returns>
        public authapi.Client.ApiResponse<UserInfo> GetUserInfoByEmailWithHttpInfo(string email, int operationIndex = 0)
        {
            // verify the required parameter 'email' is set
            if (email == null)
            {
                throw new authapi.Client.ApiException(400, "Missing required parameter 'email' when calling UserInfoApi->GetUserInfoByEmail");
            }

            authapi.Client.RequestOptions localVarRequestOptions = new authapi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = authapi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = authapi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.QueryParameters.Add(authapi.Client.ClientUtils.ParameterToMultiMap("", "email", email));

            localVarRequestOptions.Operation = "UserInfoApi.GetUserInfoByEmail";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = this.Client.Get<UserInfo>("/userinfo/search/email", localVarRequestOptions, this.Configuration);
            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserInfoByEmail", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

        /// <summary>
        /// Get User Info by Email Get user information by email address. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of UserInfo</returns>
        public async System.Threading.Tasks.Task<UserInfo> GetUserInfoByEmailAsync(string email, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            authapi.Client.ApiResponse<UserInfo> localVarResponse = await GetUserInfoByEmailWithHttpInfoAsync(email, operationIndex, cancellationToken).ConfigureAwait(false);
            return localVarResponse.Data;
        }

        /// <summary>
        /// Get User Info by Email Get user information by email address. 
        /// </summary>
        /// <exception cref="authapi.Client.ApiException">Thrown when fails to make API call</exception>
        /// <param name="email">Email</param>
        /// <param name="operationIndex">Index associated with the operation.</param>
        /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
        /// <returns>Task of ApiResponse (UserInfo)</returns>
        public async System.Threading.Tasks.Task<authapi.Client.ApiResponse<UserInfo>> GetUserInfoByEmailWithHttpInfoAsync(string email, int operationIndex = 0, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // verify the required parameter 'email' is set
            if (email == null)
            {
                throw new authapi.Client.ApiException(400, "Missing required parameter 'email' when calling UserInfoApi->GetUserInfoByEmail");
            }


            authapi.Client.RequestOptions localVarRequestOptions = new authapi.Client.RequestOptions();

            string[] _contentTypes = new string[] {
            };

            // to determine the Accept header
            string[] _accepts = new string[] {
                "application/json"
            };

            var localVarContentType = authapi.Client.ClientUtils.SelectHeaderContentType(_contentTypes);
            if (localVarContentType != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Content-Type", localVarContentType);
            }

            var localVarAccept = authapi.Client.ClientUtils.SelectHeaderAccept(_accepts);
            if (localVarAccept != null)
            {
                localVarRequestOptions.HeaderParameters.Add("Accept", localVarAccept);
            }

            localVarRequestOptions.QueryParameters.Add(authapi.Client.ClientUtils.ParameterToMultiMap("", "email", email));

            localVarRequestOptions.Operation = "UserInfoApi.GetUserInfoByEmail";
            localVarRequestOptions.OperationIndex = operationIndex;

            // authentication (Bearer) required
            // bearer authentication required
            if (!string.IsNullOrEmpty(this.Configuration.AccessToken) && !localVarRequestOptions.HeaderParameters.ContainsKey("Authorization"))
            {
                localVarRequestOptions.HeaderParameters.Add("Authorization", "Bearer " + this.Configuration.AccessToken);
            }

            // make the HTTP request
            var localVarResponse = await this.AsynchronousClient.GetAsync<UserInfo>("/userinfo/search/email", localVarRequestOptions, this.Configuration, cancellationToken).ConfigureAwait(false);

            if (this.ExceptionFactory != null)
            {
                Exception _exception = this.ExceptionFactory("GetUserInfoByEmail", localVarResponse);
                if (_exception != null)
                {
                    throw _exception;
                }
            }

            return localVarResponse;
        }

    }
}
