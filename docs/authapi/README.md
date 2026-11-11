# authapi - the C# library for the SaaSus Auth API Schema

Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/auth*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AuthInfoApi* | [**GetAuthInfo**](docs/AuthInfoApi.md#getauthinfo) | **GET** /auth-info | Get Authentication Info
*AuthInfoApi* | [**GetIdentityProviders**](docs/AuthInfoApi.md#getidentityproviders) | **GET** /identity-providers | Get Sign-In Information Via External Provider
*AuthInfoApi* | [**GetSignInSettings**](docs/AuthInfoApi.md#getsigninsettings) | **GET** /sign-in-settings | Get Sign-In Settings
*AuthInfoApi* | [**UpdateAuthInfo**](docs/AuthInfoApi.md#updateauthinfo) | **PUT** /auth-info | Update Authentication Info
*AuthInfoApi* | [**UpdateIdentityProvider**](docs/AuthInfoApi.md#updateidentityprovider) | **PUT** /identity-providers | Update Sign-In Information
*AuthInfoApi* | [**UpdateSignInSettings**](docs/AuthInfoApi.md#updatesigninsettings) | **PUT** /sign-in-settings | Update Sign-In Settings
*BasicInfoApi* | [**FindNotificationMessages**](docs/BasicInfoApi.md#findnotificationmessages) | **GET** /notification-messages | Get Notification Email Templates
*BasicInfoApi* | [**GetBasicInfo**](docs/BasicInfoApi.md#getbasicinfo) | **GET** /basic-info | Get Basic Configurations
*BasicInfoApi* | [**GetCustomizePageSettings**](docs/BasicInfoApi.md#getcustomizepagesettings) | **GET** /customize-page-settings | Get Authentication Authorization Basic Information
*BasicInfoApi* | [**GetCustomizePages**](docs/BasicInfoApi.md#getcustomizepages) | **GET** /customize-pages | Get Authentication Page Setting
*BasicInfoApi* | [**UpdateBasicInfo**](docs/BasicInfoApi.md#updatebasicinfo) | **PUT** /basic-info | Update Basic Configurations
*BasicInfoApi* | [**UpdateCustomizePageSettings**](docs/BasicInfoApi.md#updatecustomizepagesettings) | **PATCH** /customize-page-settings | Update Authentication Authorization Basic Information
*BasicInfoApi* | [**UpdateCustomizePages**](docs/BasicInfoApi.md#updatecustomizepages) | **PATCH** /customize-pages | Authentication Page Setting
*BasicInfoApi* | [**UpdateNotificationMessages**](docs/BasicInfoApi.md#updatenotificationmessages) | **PUT** /notification-messages | Update Notification Email Template
*CredentialApi* | [**CreateAuthCredentials**](docs/CredentialApi.md#createauthcredentials) | **POST** /credentials | Save Authentication/Authorization Information
*CredentialApi* | [**ExchangeAuthCredentials**](docs/CredentialApi.md#exchangeauthcredentials) | **POST** /credentials/exchange | Exchange a Temporary Code for Authentication/Authorization Information
*CredentialApi* | [**GetAuthCredentials**](docs/CredentialApi.md#getauthcredentials) | **GET** /credentials | Get Authentication/Authorization Information
*CredentialApi* | [**RevokeToken**](docs/CredentialApi.md#revoketoken) | **POST** /token/revoke | Revoke Token
*EnvApi* | [**CreateEnv**](docs/EnvApi.md#createenv) | **POST** /envs | Create Env Info
*EnvApi* | [**DeleteEnv**](docs/EnvApi.md#deleteenv) | **DELETE** /envs/{env_id} | Delete Env Info
*EnvApi* | [**GetEnv**](docs/EnvApi.md#getenv) | **GET** /envs/{env_id} | Get Env Details
*EnvApi* | [**GetEnvs**](docs/EnvApi.md#getenvs) | **GET** /envs | Get Env Info
*EnvApi* | [**UpdateEnv**](docs/EnvApi.md#updateenv) | **PATCH** /envs/{env_id} | Update Env Info
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error
*InvitationApi* | [**CreateTenantInvitation**](docs/InvitationApi.md#createtenantinvitation) | **POST** /tenants/{tenant_id}/invitations | Create Tenant Invitation
*InvitationApi* | [**DeleteTenantInvitation**](docs/InvitationApi.md#deletetenantinvitation) | **DELETE** /tenants/{tenant_id}/invitations/{invitation_id} | Delete Tenant Invitation
*InvitationApi* | [**GetInvitationValidity**](docs/InvitationApi.md#getinvitationvalidity) | **GET** /invitations/{invitation_id}/validity | Get Invitation Validity
*InvitationApi* | [**GetTenantInvitation**](docs/InvitationApi.md#gettenantinvitation) | **GET** /tenants/{tenant_id}/invitations/{invitation_id} | Get Tenant Invitation
*InvitationApi* | [**GetTenantInvitations**](docs/InvitationApi.md#gettenantinvitations) | **GET** /tenants/{tenant_id}/invitations | Get Tenant Invitations
*InvitationApi* | [**ValidateInvitation**](docs/InvitationApi.md#validateinvitation) | **PATCH** /invitations/{invitation_id}/validate | Validate Invitation
*RoleApi* | [**CreateRole**](docs/RoleApi.md#createrole) | **POST** /roles | Create Role
*RoleApi* | [**DeleteRole**](docs/RoleApi.md#deleterole) | **DELETE** /roles/{role_name} | Delete Role
*RoleApi* | [**GetRoles**](docs/RoleApi.md#getroles) | **GET** /roles | Get Roles
*RoleApi* | [**UpdateRole**](docs/RoleApi.md#updaterole) | **PATCH** /roles/{role_name} | Update Role
*SaasUserApi* | [**ConfirmDevice**](docs/SaasUserApi.md#confirmdevice) | **POST** /device/confirm | Confirm Device
*SaasUserApi* | [**ConfirmEmailUpdate**](docs/SaasUserApi.md#confirmemailupdate) | **POST** /users/{user_id}/email/confirm | Confirm User Email Update
*SaasUserApi* | [**ConfirmExternalUserLink**](docs/SaasUserApi.md#confirmexternaluserlink) | **POST** /external-users/confirm | Confirm External User Account Link
*SaasUserApi* | [**ConfirmSignUpWithAwsMarketplace**](docs/SaasUserApi.md#confirmsignupwithawsmarketplace) | **POST** /aws-marketplace/sign-up-confirm | Confirm Sign Up with AWS Marketplace
*SaasUserApi* | [**CreateSaasUser**](docs/SaasUserApi.md#createsaasuser) | **POST** /users | Create SaaS User
*SaasUserApi* | [**CreateSecretCode**](docs/SaasUserApi.md#createsecretcode) | **POST** /users/{user_id}/mfa/software-token/secret-code | Create secret code for authentication application registration
*SaasUserApi* | [**DeleteSaasUser**](docs/SaasUserApi.md#deletesaasuser) | **DELETE** /users/{user_id} | Delete User
*SaasUserApi* | [**GetSaasUser**](docs/SaasUserApi.md#getsaasuser) | **GET** /users/{user_id} | Get User
*SaasUserApi* | [**GetSaasUsers**](docs/SaasUserApi.md#getsaasusers) | **GET** /users | Get Users
*SaasUserApi* | [**GetSaasUsersCount**](docs/SaasUserApi.md#getsaasuserscount) | **GET** /users/count | Get SaaS Users Count
*SaasUserApi* | [**GetUserMfaPreference**](docs/SaasUserApi.md#getusermfapreference) | **GET** /users/{user_id}/mfa/preference | Get User's MFA Settings
*SaasUserApi* | [**LinkAwsMarketplace**](docs/SaasUserApi.md#linkawsmarketplace) | **PATCH** /aws-marketplace/link | Link an existing tenant with AWS Marketplace
*SaasUserApi* | [**RequestEmailUpdate**](docs/SaasUserApi.md#requestemailupdate) | **POST** /users/{user_id}/email/request | Request User Email Update
*SaasUserApi* | [**RequestExternalUserLink**](docs/SaasUserApi.md#requestexternaluserlink) | **POST** /external-users/request | Request External User Account Link
*SaasUserApi* | [**ResendSignUpConfirmationEmail**](docs/SaasUserApi.md#resendsignupconfirmationemail) | **POST** /sign-up/resend | Resend Sign Up Confirmation Email
*SaasUserApi* | [**ResetSaasUserPassword**](docs/SaasUserApi.md#resetsaasuserpassword) | **POST** /users/{user_id}/password/reset | Reset Password
*SaasUserApi* | [**RespondToSignInChallenge**](docs/SaasUserApi.md#respondtosigninchallenge) | **POST** /sign-in/challenge | Respond to Sign In Challenge
*SaasUserApi* | [**SaveSaasUsersCount**](docs/SaasUserApi.md#savesaasuserscount) | **POST** /users/count | Save SaaS Users Count
*SaasUserApi* | [**SearchSaasUsers**](docs/SaasUserApi.md#searchsaasusers) | **GET** /users/search | Search SaaS Users
*SaasUserApi* | [**SignIn**](docs/SaasUserApi.md#signin) | **POST** /sign-in | Sign In
*SaasUserApi* | [**SignUp**](docs/SaasUserApi.md#signup) | **POST** /sign-up | Sign Up
*SaasUserApi* | [**SignUpWithAwsMarketplace**](docs/SaasUserApi.md#signupwithawsmarketplace) | **POST** /aws-marketplace/sign-up | Sign Up with AWS Marketplace
*SaasUserApi* | [**UnlinkProvider**](docs/SaasUserApi.md#unlinkprovider) | **DELETE** /users/{user_id}/providers/{provider_name} | Unlink external identity providers
*SaasUserApi* | [**UpdateDeviceStatus**](docs/SaasUserApi.md#updatedevicestatus) | **POST** /device/status | Update Device Status
*SaasUserApi* | [**UpdateSaasUserAttributes**](docs/SaasUserApi.md#updatesaasuserattributes) | **PATCH** /users/{user_id}/attributes | Update SaaS User Attributes
*SaasUserApi* | [**UpdateSaasUserEmail**](docs/SaasUserApi.md#updatesaasuseremail) | **PATCH** /users/{user_id}/email | Change Email
*SaasUserApi* | [**UpdateSaasUserPassword**](docs/SaasUserApi.md#updatesaasuserpassword) | **PATCH** /users/{user_id}/password | Change Password
*SaasUserApi* | [**UpdateSaasUserSignInId**](docs/SaasUserApi.md#updatesaasusersigninid) | **PATCH** /users/{user_id}/sign-in-id | Change Sign-in ID
*SaasUserApi* | [**UpdateSoftwareToken**](docs/SaasUserApi.md#updatesoftwaretoken) | **PUT** /users/{user_id}/mfa/software-token | Register Authentication Application
*SaasUserApi* | [**UpdateUserMfaPreference**](docs/SaasUserApi.md#updateusermfapreference) | **PATCH** /users/{user_id}/mfa/preference | Update User's MFA Settings
*SingleTenantApi* | [**GetCloudFormationLaunchStackLinkForSingleTenant**](docs/SingleTenantApi.md#getcloudformationlaunchstacklinkforsingletenant) | **GET** /single-tenant/cloudformation-launch-stack-link | Get CloudFormation Stack Launch Link For SaaS Infrastructure Management
*SingleTenantApi* | [**GetSingleTenantSettings**](docs/SingleTenantApi.md#getsingletenantsettings) | **GET** /single-tenant/settings | Retrieve the settings of the SaaS Infrastructure Management.
*SingleTenantApi* | [**UpdateSingleTenantSettings**](docs/SingleTenantApi.md#updatesingletenantsettings) | **PATCH** /single-tenant/settings | Update configuration information for SaaS Infrastructure Management
*TenantApi* | [**CreateTenant**](docs/TenantApi.md#createtenant) | **POST** /tenants | Create Tenant
*TenantApi* | [**CreateTenantAndPricing**](docs/TenantApi.md#createtenantandpricing) | **PATCH** /stripe/init | Stripe Initial Setting
*TenantApi* | [**DeleteStripeTenantAndPricing**](docs/TenantApi.md#deletestripetenantandpricing) | **DELETE** /stripe | Delete Customer and Product From Stripe
*TenantApi* | [**DeleteTenant**](docs/TenantApi.md#deletetenant) | **DELETE** /tenants/{tenant_id} | Delete Tenant
*TenantApi* | [**GetStripeCustomer**](docs/TenantApi.md#getstripecustomer) | **GET** /tenants/{tenant_id}/stripe-customer | Get Stripe Customer
*TenantApi* | [**GetTenant**](docs/TenantApi.md#gettenant) | **GET** /tenants/{tenant_id} | Get Tenant Details
*TenantApi* | [**GetTenantIdentityProviders**](docs/TenantApi.md#gettenantidentityproviders) | **GET** /tenants/{tenant_id}/identity-providers | Get identity provider per tenant
*TenantApi* | [**GetTenants**](docs/TenantApi.md#gettenants) | **GET** /tenants | Get Tenants
*TenantApi* | [**ResetPlan**](docs/TenantApi.md#resetplan) | **PUT** /plans/reset | Delete all information related to rate plans
*TenantApi* | [**UpdateTenant**](docs/TenantApi.md#updatetenant) | **PATCH** /tenants/{tenant_id} | Update Tenant Details
*TenantApi* | [**UpdateTenantBillingInfo**](docs/TenantApi.md#updatetenantbillinginfo) | **PUT** /tenants/{tenant_id}/billing-info | Update Tenant Billing Information
*TenantApi* | [**UpdateTenantIdentityProvider**](docs/TenantApi.md#updatetenantidentityprovider) | **PUT** /tenants/{tenant_id}/identity-providers | Update identity provider per tenant
*TenantApi* | [**UpdateTenantPlan**](docs/TenantApi.md#updatetenantplan) | **PUT** /tenants/{tenant_id}/plans | Update Tenant Plan Information
*TenantAttributeApi* | [**CreateTenantAttribute**](docs/TenantAttributeApi.md#createtenantattribute) | **POST** /tenant-attributes | Create Tenant Attribute
*TenantAttributeApi* | [**DeleteTenantAttribute**](docs/TenantAttributeApi.md#deletetenantattribute) | **DELETE** /tenant-attributes/{attribute_name} | Delete Tenant Attribute
*TenantAttributeApi* | [**GetTenantAttributes**](docs/TenantAttributeApi.md#gettenantattributes) | **GET** /tenant-attributes | Get Tenant Attributes
*TenantUserApi* | [**CreateTenantUser**](docs/TenantUserApi.md#createtenantuser) | **POST** /tenants/{tenant_id}/users | Create Tenant User
*TenantUserApi* | [**CreateTenantUserRoles**](docs/TenantUserApi.md#createtenantuserroles) | **POST** /tenants/{tenant_id}/users/{user_id}/envs/{env_id}/roles | Create Tenant User Role
*TenantUserApi* | [**DeleteTenantUser**](docs/TenantUserApi.md#deletetenantuser) | **DELETE** /tenants/{tenant_id}/users/{user_id} | Delete Tenant User
*TenantUserApi* | [**DeleteTenantUserRole**](docs/TenantUserApi.md#deletetenantuserrole) | **DELETE** /tenants/{tenant_id}/users/{user_id}/envs/{env_id}/roles/{role_name} | Remove Role From Tenant User
*TenantUserApi* | [**GetAllTenantUser**](docs/TenantUserApi.md#getalltenantuser) | **GET** /tenants/all/users/{user_id} | Get User Info
*TenantUserApi* | [**GetAllTenantUsers**](docs/TenantUserApi.md#getalltenantusers) | **GET** /tenants/all/users | Get Users
*TenantUserApi* | [**GetAllTenantUsersCount**](docs/TenantUserApi.md#getalltenantuserscount) | **GET** /tenants/all/users/count | Get Tenant Users Count
*TenantUserApi* | [**GetTenantUser**](docs/TenantUserApi.md#gettenantuser) | **GET** /tenants/{tenant_id}/users/{user_id} | Get Tenant User
*TenantUserApi* | [**GetTenantUsers**](docs/TenantUserApi.md#gettenantusers) | **GET** /tenants/{tenant_id}/users | Get Tenant Users
*TenantUserApi* | [**SaveTenantUsersCounts**](docs/TenantUserApi.md#savetenantuserscounts) | **POST** /tenants/all/users/count | Save Tenant Users Count
*TenantUserApi* | [**SearchTenantUsers**](docs/TenantUserApi.md#searchtenantusers) | **GET** /tenants/all/users/search | Search Tenant Users
*TenantUserApi* | [**UpdateTenantUser**](docs/TenantUserApi.md#updatetenantuser) | **PATCH** /tenants/{tenant_id}/users/{user_id} | Update Tenant User Attribute
*UserAttributeApi* | [**CreateSaasUserAttribute**](docs/UserAttributeApi.md#createsaasuserattribute) | **POST** /saas-user-attributes | Create SaaS User Attributes
*UserAttributeApi* | [**CreateUserAttribute**](docs/UserAttributeApi.md#createuserattribute) | **POST** /user-attributes | Create User Attributes
*UserAttributeApi* | [**DeleteUserAttribute**](docs/UserAttributeApi.md#deleteuserattribute) | **DELETE** /user-attributes/{attribute_name} | Delete User Attribute
*UserAttributeApi* | [**GetUserAttributes**](docs/UserAttributeApi.md#getuserattributes) | **GET** /user-attributes | Get User Attributes
*UserInfoApi* | [**GetUserInfo**](docs/UserInfoApi.md#getuserinfo) | **GET** /userinfo | Get User Info
*UserInfoApi* | [**GetUserInfoByEmail**](docs/UserInfoApi.md#getuserinfobyemail) | **GET** /userinfo/search/email | Get User Info by Email
*UserInfoApi* | [**GetUserInfoBySignInId**](docs/UserInfoApi.md#getuserinfobysigninid) | **GET** /userinfo/search/sign-in-id | Get User Info by Sign-in ID


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.AccountVerification](docs/AccountVerification.md)
 - [Model.ApiKeys](docs/ApiKeys.md)
 - [Model.Attribute](docs/Attribute.md)
 - [Model.AttributeType](docs/AttributeType.md)
 - [Model.AuthInfo](docs/AuthInfo.md)
 - [Model.AuthorizationTempCode](docs/AuthorizationTempCode.md)
 - [Model.BasicInfo](docs/BasicInfo.md)
 - [Model.BillingAddress](docs/BillingAddress.md)
 - [Model.BillingInfo](docs/BillingInfo.md)
 - [Model.ChallengeName](docs/ChallengeName.md)
 - [Model.ClientSecret](docs/ClientSecret.md)
 - [Model.CloudFormationLaunchStackLink](docs/CloudFormationLaunchStackLink.md)
 - [Model.ConfirmDeviceParam](docs/ConfirmDeviceParam.md)
 - [Model.ConfirmDeviceResult](docs/ConfirmDeviceResult.md)
 - [Model.ConfirmEmailUpdateParam](docs/ConfirmEmailUpdateParam.md)
 - [Model.ConfirmExternalUserLinkParam](docs/ConfirmExternalUserLinkParam.md)
 - [Model.ConfirmSignUpWithAwsMarketplaceParam](docs/ConfirmSignUpWithAwsMarketplaceParam.md)
 - [Model.CreateAuthCredentialsParam](docs/CreateAuthCredentialsParam.md)
 - [Model.CreateSaasUserParam](docs/CreateSaasUserParam.md)
 - [Model.CreateSecretCodeParam](docs/CreateSecretCodeParam.md)
 - [Model.CreateTenantInvitationParam](docs/CreateTenantInvitationParam.md)
 - [Model.CreateTenantUserParam](docs/CreateTenantUserParam.md)
 - [Model.CreateTenantUserRolesParam](docs/CreateTenantUserRolesParam.md)
 - [Model.CreatedSaasUser](docs/CreatedSaasUser.md)
 - [Model.Credentials](docs/Credentials.md)
 - [Model.CustomizePageProps](docs/CustomizePageProps.md)
 - [Model.CustomizePageSettings](docs/CustomizePageSettings.md)
 - [Model.CustomizePageSettingsProps](docs/CustomizePageSettingsProps.md)
 - [Model.CustomizePages](docs/CustomizePages.md)
 - [Model.DeviceConfiguration](docs/DeviceConfiguration.md)
 - [Model.DeviceRememberedStatus](docs/DeviceRememberedStatus.md)
 - [Model.DeviceSecretVerifierConfig](docs/DeviceSecretVerifierConfig.md)
 - [Model.DnsRecord](docs/DnsRecord.md)
 - [Model.Env](docs/Env.md)
 - [Model.Envs](docs/Envs.md)
 - [Model.Error](docs/Error.md)
 - [Model.ExchangeAuthCredentialsParam](docs/ExchangeAuthCredentialsParam.md)
 - [Model.IdentityProviderConfiguration](docs/IdentityProviderConfiguration.md)
 - [Model.IdentityProviderProps](docs/IdentityProviderProps.md)
 - [Model.IdentityProviderSaml](docs/IdentityProviderSaml.md)
 - [Model.IdentityProviders](docs/IdentityProviders.md)
 - [Model.Invitation](docs/Invitation.md)
 - [Model.InvitationStatus](docs/InvitationStatus.md)
 - [Model.InvitationValidity](docs/InvitationValidity.md)
 - [Model.Invitations](docs/Invitations.md)
 - [Model.InvitedUserEnvironmentInformationInner](docs/InvitedUserEnvironmentInformationInner.md)
 - [Model.InvoiceLanguage](docs/InvoiceLanguage.md)
 - [Model.LinkAwsMarketplaceParam](docs/LinkAwsMarketplaceParam.md)
 - [Model.MessageTemplate](docs/MessageTemplate.md)
 - [Model.MfaConfiguration](docs/MfaConfiguration.md)
 - [Model.MfaPreference](docs/MfaPreference.md)
 - [Model.NewDeviceMetadata](docs/NewDeviceMetadata.md)
 - [Model.NotificationMessages](docs/NotificationMessages.md)
 - [Model.PasswordPolicy](docs/PasswordPolicy.md)
 - [Model.PlanHistories](docs/PlanHistories.md)
 - [Model.PlanHistory](docs/PlanHistory.md)
 - [Model.PlanReservation](docs/PlanReservation.md)
 - [Model.ProrationBehavior](docs/ProrationBehavior.md)
 - [Model.ProviderName](docs/ProviderName.md)
 - [Model.ProviderType](docs/ProviderType.md)
 - [Model.RecaptchaProps](docs/RecaptchaProps.md)
 - [Model.RefreshTokenValidity](docs/RefreshTokenValidity.md)
 - [Model.RefreshTokenValidityUnit](docs/RefreshTokenValidityUnit.md)
 - [Model.RequestEmailUpdateParam](docs/RequestEmailUpdateParam.md)
 - [Model.RequestExternalUserLinkParam](docs/RequestExternalUserLinkParam.md)
 - [Model.ResendSignUpConfirmationEmailParam](docs/ResendSignUpConfirmationEmailParam.md)
 - [Model.RespondToSignInChallengeParam](docs/RespondToSignInChallengeParam.md)
 - [Model.RespondToSignInChallengeResult](docs/RespondToSignInChallengeResult.md)
 - [Model.RevokeTokenParam](docs/RevokeTokenParam.md)
 - [Model.Role](docs/Role.md)
 - [Model.Roles](docs/Roles.md)
 - [Model.SaasId](docs/SaasId.md)
 - [Model.SaasUser](docs/SaasUser.md)
 - [Model.SaasUserResetPasswordResult](docs/SaasUserResetPasswordResult.md)
 - [Model.SaasUsers](docs/SaasUsers.md)
 - [Model.SaasUsersCount](docs/SaasUsersCount.md)
 - [Model.SaveSaasUsersCountParam](docs/SaveSaasUsersCountParam.md)
 - [Model.SaveTenantUserCountParam](docs/SaveTenantUserCountParam.md)
 - [Model.SaveTenantUsersCountsParam](docs/SaveTenantUsersCountsParam.md)
 - [Model.SearchSaasUsersResult](docs/SearchSaasUsersResult.md)
 - [Model.SearchTenantUsersResult](docs/SearchTenantUsersResult.md)
 - [Model.SelfRegist](docs/SelfRegist.md)
 - [Model.SignInParam](docs/SignInParam.md)
 - [Model.SignInResult](docs/SignInResult.md)
 - [Model.SignInSettings](docs/SignInSettings.md)
 - [Model.SignUpParam](docs/SignUpParam.md)
 - [Model.SignUpWithAwsMarketplaceParam](docs/SignUpWithAwsMarketplaceParam.md)
 - [Model.SingleTenantSettings](docs/SingleTenantSettings.md)
 - [Model.SoftwareTokenSecretCode](docs/SoftwareTokenSecretCode.md)
 - [Model.StripeCustomer](docs/StripeCustomer.md)
 - [Model.Tenant](docs/Tenant.md)
 - [Model.TenantAttributes](docs/TenantAttributes.md)
 - [Model.TenantDetail](docs/TenantDetail.md)
 - [Model.TenantIdentityProviderProps](docs/TenantIdentityProviderProps.md)
 - [Model.TenantIdentityProviders](docs/TenantIdentityProviders.md)
 - [Model.TenantIdentityProvidersSaml](docs/TenantIdentityProvidersSaml.md)
 - [Model.TenantProps](docs/TenantProps.md)
 - [Model.TenantUserCount](docs/TenantUserCount.md)
 - [Model.TenantUsersCounts](docs/TenantUsersCounts.md)
 - [Model.Tenants](docs/Tenants.md)
 - [Model.UpdateBasicInfoParam](docs/UpdateBasicInfoParam.md)
 - [Model.UpdateCustomizePageSettingsParam](docs/UpdateCustomizePageSettingsParam.md)
 - [Model.UpdateCustomizePagesParam](docs/UpdateCustomizePagesParam.md)
 - [Model.UpdateDeviceStatusParam](docs/UpdateDeviceStatusParam.md)
 - [Model.UpdateEnvParam](docs/UpdateEnvParam.md)
 - [Model.UpdateIdentityProviderParam](docs/UpdateIdentityProviderParam.md)
 - [Model.UpdateNotificationMessagesParam](docs/UpdateNotificationMessagesParam.md)
 - [Model.UpdateRoleParam](docs/UpdateRoleParam.md)
 - [Model.UpdateSaasUserAttributesParam](docs/UpdateSaasUserAttributesParam.md)
 - [Model.UpdateSaasUserEmailParam](docs/UpdateSaasUserEmailParam.md)
 - [Model.UpdateSaasUserPasswordParam](docs/UpdateSaasUserPasswordParam.md)
 - [Model.UpdateSaasUserSignInIdParam](docs/UpdateSaasUserSignInIdParam.md)
 - [Model.UpdateSignInSettingsParam](docs/UpdateSignInSettingsParam.md)
 - [Model.UpdateSingleTenantSettingsParam](docs/UpdateSingleTenantSettingsParam.md)
 - [Model.UpdateSoftwareTokenParam](docs/UpdateSoftwareTokenParam.md)
 - [Model.UpdateTenantIdentityProviderParam](docs/UpdateTenantIdentityProviderParam.md)
 - [Model.UpdateTenantUserParam](docs/UpdateTenantUserParam.md)
 - [Model.User](docs/User.md)
 - [Model.UserAttributes](docs/UserAttributes.md)
 - [Model.UserAvailableEnv](docs/UserAvailableEnv.md)
 - [Model.UserAvailableTenant](docs/UserAvailableTenant.md)
 - [Model.UserInfo](docs/UserInfo.md)
 - [Model.Users](docs/Users.md)
 - [Model.ValidateInvitationParam](docs/ValidateInvitationParam.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


