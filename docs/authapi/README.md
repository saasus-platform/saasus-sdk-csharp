# authapi - the C# library for the SaaSus Auth API Schema

Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/auth*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AuthInfoApi* | [**GetAuthInfo**](docs/AuthInfoApi.md#getauthinfo) | **GET** /auth-info | Get Authentication Info
*AuthInfoApi* | [**GetIdentityProviders**](docs/AuthInfoApi.md#getidentityproviders) | **GET** /identity-providers | Get Sign-In Information Via External Provider
*AuthInfoApi* | [**GetSignInSettings**](docs/AuthInfoApi.md#getsigninsettings) | **GET** /sign-in-settings | Get Password Requirements
*AuthInfoApi* | [**UpdateAuthInfo**](docs/AuthInfoApi.md#updateauthinfo) | **PUT** /auth-info | Update Authentication Info
*AuthInfoApi* | [**UpdateIdentityProvider**](docs/AuthInfoApi.md#updateidentityprovider) | **PUT** /identity-providers | Update Sign-In Information
*AuthInfoApi* | [**UpdateSignInSettings**](docs/AuthInfoApi.md#updatesigninsettings) | **PUT** /sign-in-settings | Update Password Requirements
*BasicInfoApi* | [**FindNotificationMessages**](docs/BasicInfoApi.md#findnotificationmessages) | **GET** /notification-messages | Get Notification Email Templates
*BasicInfoApi* | [**GetBasicInfo**](docs/BasicInfoApi.md#getbasicinfo) | **GET** /basic-info | Get Basic Configurations
*BasicInfoApi* | [**GetCustomizePageSettings**](docs/BasicInfoApi.md#getcustomizepagesettings) | **GET** /customize-page-settings | Get Authentication Authorization Basic Information
*BasicInfoApi* | [**GetCustomizePages**](docs/BasicInfoApi.md#getcustomizepages) | **GET** /customize-pages | Get Authentication Page Setting
*BasicInfoApi* | [**UpdateBasicInfo**](docs/BasicInfoApi.md#updatebasicinfo) | **PUT** /basic-info | Update Basic Configurations
*BasicInfoApi* | [**UpdateCustomizePageSettings**](docs/BasicInfoApi.md#updatecustomizepagesettings) | **PATCH** /customize-page-settings | Update Authentication Authorization Basic Information
*BasicInfoApi* | [**UpdateCustomizePages**](docs/BasicInfoApi.md#updatecustomizepages) | **PATCH** /customize-pages | Authentication Page Setting
*BasicInfoApi* | [**UpdateNotificationMessages**](docs/BasicInfoApi.md#updatenotificationmessages) | **PUT** /notification-messages | Update Notification Email Template
*CredentialApi* | [**CreateAuthCredentials**](docs/CredentialApi.md#createauthcredentials) | **POST** /credentials | Save Authentication/Authorization Information
*CredentialApi* | [**GetAuthCredentials**](docs/CredentialApi.md#getauthcredentials) | **GET** /credentials | Get Authentication/Authorization Information
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
*SaasUserApi* | [**ConfirmEmailUpdate**](docs/SaasUserApi.md#confirmemailupdate) | **POST** /users/{user_id}/email/confirm | Confirm User Email Update
*SaasUserApi* | [**ConfirmExternalUserLink**](docs/SaasUserApi.md#confirmexternaluserlink) | **POST** /external-users/confirm | Confirm External User Account Link
*SaasUserApi* | [**ConfirmSignUpWithAwsMarketplace**](docs/SaasUserApi.md#confirmsignupwithawsmarketplace) | **POST** /aws-marketplace/sign-up-confirm | Confirm Sign Up with AWS Marketplace
*SaasUserApi* | [**CreateSaasUser**](docs/SaasUserApi.md#createsaasuser) | **POST** /users | Create SaaS User
*SaasUserApi* | [**CreateSecretCode**](docs/SaasUserApi.md#createsecretcode) | **POST** /users/{user_id}/mfa/software-token/secret-code | Create secret code for authentication application registration
*SaasUserApi* | [**DeleteSaasUser**](docs/SaasUserApi.md#deletesaasuser) | **DELETE** /users/{user_id} | Delete User
*SaasUserApi* | [**GetSaasUser**](docs/SaasUserApi.md#getsaasuser) | **GET** /users/{user_id} | Get User
*SaasUserApi* | [**GetSaasUsers**](docs/SaasUserApi.md#getsaasusers) | **GET** /users | Get Users
*SaasUserApi* | [**GetUserMfaPreference**](docs/SaasUserApi.md#getusermfapreference) | **GET** /users/{user_id}/mfa/preference | Get User's MFA Settings
*SaasUserApi* | [**LinkAwsMarketplace**](docs/SaasUserApi.md#linkawsmarketplace) | **PATCH** /aws-marketplace/link | Link an existing tenant with AWS Marketplace
*SaasUserApi* | [**RequestEmailUpdate**](docs/SaasUserApi.md#requestemailupdate) | **POST** /users/{user_id}/email/request | Request User Email Update
*SaasUserApi* | [**RequestExternalUserLink**](docs/SaasUserApi.md#requestexternaluserlink) | **POST** /external-users/request | Request External User Account Link
*SaasUserApi* | [**ResendSignUpConfirmationEmail**](docs/SaasUserApi.md#resendsignupconfirmationemail) | **POST** /sign-up/resend | Resend Sign Up Confirmation Email
*SaasUserApi* | [**SignUp**](docs/SaasUserApi.md#signup) | **POST** /sign-up | Sign Up
*SaasUserApi* | [**SignUpWithAwsMarketplace**](docs/SaasUserApi.md#signupwithawsmarketplace) | **POST** /aws-marketplace/sign-up | Sign Up with AWS Marketplace
*SaasUserApi* | [**UnlinkProvider**](docs/SaasUserApi.md#unlinkprovider) | **DELETE** /users/{user_id}/providers/{provider_name} | Unlink external identity providers
*SaasUserApi* | [**UpdateSaasUserAttributes**](docs/SaasUserApi.md#updatesaasuserattributes) | **PATCH** /users/{user_id}/attributes | Update SaaS User Attributes
*SaasUserApi* | [**UpdateSaasUserEmail**](docs/SaasUserApi.md#updatesaasuseremail) | **PATCH** /users/{user_id}/email | Change Email
*SaasUserApi* | [**UpdateSaasUserPassword**](docs/SaasUserApi.md#updatesaasuserpassword) | **PATCH** /users/{user_id}/password | Change Password
*SaasUserApi* | [**UpdateSoftwareToken**](docs/SaasUserApi.md#updatesoftwaretoken) | **PUT** /users/{user_id}/mfa/software-token | Register Authentication Application
*SaasUserApi* | [**UpdateUserMfaPreference**](docs/SaasUserApi.md#updateusermfapreference) | **PATCH** /users/{user_id}/mfa/preference | Update User's MFA Settings
*SingleTenantApi* | [**GetCloudFormationLaunchStackLinkForSingleTenant**](docs/SingleTenantApi.md#getcloudformationlaunchstacklinkforsingletenant) | **GET** /single-tenant/cloudformation-launch-stack-link | Get CloudFormation Stack Launch Link For Single Tenant
*SingleTenantApi* | [**GetSingleTenantSettings**](docs/SingleTenantApi.md#getsingletenantsettings) | **GET** /single-tenant/settings | Retrieve the settings of the single tenant.
*SingleTenantApi* | [**UpdateSingleTenantSettings**](docs/SingleTenantApi.md#updatesingletenantsettings) | **PATCH** /single-tenant/settings | Update configuration information for single-tenant functionality
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
*TenantUserApi* | [**GetTenantUser**](docs/TenantUserApi.md#gettenantuser) | **GET** /tenants/{tenant_id}/users/{user_id} | Get Tenant User
*TenantUserApi* | [**GetTenantUsers**](docs/TenantUserApi.md#gettenantusers) | **GET** /tenants/{tenant_id}/users | Get Tenant Users
*TenantUserApi* | [**UpdateTenantUser**](docs/TenantUserApi.md#updatetenantuser) | **PATCH** /tenants/{tenant_id}/users/{user_id} | Update Tenant User Attribute
*UserAttributeApi* | [**CreateSaasUserAttribute**](docs/UserAttributeApi.md#createsaasuserattribute) | **POST** /saas-user-attributes | Create SaaS User Attributes
*UserAttributeApi* | [**CreateUserAttribute**](docs/UserAttributeApi.md#createuserattribute) | **POST** /user-attributes | Create User Attributes
*UserAttributeApi* | [**DeleteUserAttribute**](docs/UserAttributeApi.md#deleteuserattribute) | **DELETE** /user-attributes/{attribute_name} | Delete User Attribute
*UserAttributeApi* | [**GetUserAttributes**](docs/UserAttributeApi.md#getuserattributes) | **GET** /user-attributes | Get User Attributes
*UserInfoApi* | [**GetUserInfo**](docs/UserInfoApi.md#getuserinfo) | **GET** /userinfo | Get User Info
*UserInfoApi* | [**GetUserInfoByEmail**](docs/UserInfoApi.md#getuserinfobyemail) | **GET** /userinfo/search/email | Get User Info by Email


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
 - [Model.ClientSecret](docs/ClientSecret.md)
 - [Model.CloudFormationLaunchStackLink](docs/CloudFormationLaunchStackLink.md)
 - [Model.ConfirmEmailUpdateParam](docs/ConfirmEmailUpdateParam.md)
 - [Model.ConfirmExternalUserLinkParam](docs/ConfirmExternalUserLinkParam.md)
 - [Model.ConfirmSignUpWithAwsMarketplaceParam](docs/ConfirmSignUpWithAwsMarketplaceParam.md)
 - [Model.CreateSaasUserParam](docs/CreateSaasUserParam.md)
 - [Model.CreateSecretCodeParam](docs/CreateSecretCodeParam.md)
 - [Model.CreateTenantInvitationParam](docs/CreateTenantInvitationParam.md)
 - [Model.CreateTenantUserParam](docs/CreateTenantUserParam.md)
 - [Model.CreateTenantUserRolesParam](docs/CreateTenantUserRolesParam.md)
 - [Model.Credentials](docs/Credentials.md)
 - [Model.CustomizePageProps](docs/CustomizePageProps.md)
 - [Model.CustomizePageSettings](docs/CustomizePageSettings.md)
 - [Model.CustomizePageSettingsProps](docs/CustomizePageSettingsProps.md)
 - [Model.CustomizePages](docs/CustomizePages.md)
 - [Model.DeviceConfiguration](docs/DeviceConfiguration.md)
 - [Model.DnsRecord](docs/DnsRecord.md)
 - [Model.Env](docs/Env.md)
 - [Model.Envs](docs/Envs.md)
 - [Model.Error](docs/Error.md)
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
 - [Model.NotificationMessages](docs/NotificationMessages.md)
 - [Model.PasswordPolicy](docs/PasswordPolicy.md)
 - [Model.PlanHistories](docs/PlanHistories.md)
 - [Model.PlanHistory](docs/PlanHistory.md)
 - [Model.PlanReservation](docs/PlanReservation.md)
 - [Model.ProrationBehavior](docs/ProrationBehavior.md)
 - [Model.ProviderName](docs/ProviderName.md)
 - [Model.ProviderType](docs/ProviderType.md)
 - [Model.RecaptchaProps](docs/RecaptchaProps.md)
 - [Model.RequestEmailUpdateParam](docs/RequestEmailUpdateParam.md)
 - [Model.RequestExternalUserLinkParam](docs/RequestExternalUserLinkParam.md)
 - [Model.ResendSignUpConfirmationEmailParam](docs/ResendSignUpConfirmationEmailParam.md)
 - [Model.Role](docs/Role.md)
 - [Model.Roles](docs/Roles.md)
 - [Model.SaasId](docs/SaasId.md)
 - [Model.SaasUser](docs/SaasUser.md)
 - [Model.SaasUsers](docs/SaasUsers.md)
 - [Model.SelfRegist](docs/SelfRegist.md)
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
 - [Model.Tenants](docs/Tenants.md)
 - [Model.UpdateBasicInfoParam](docs/UpdateBasicInfoParam.md)
 - [Model.UpdateCustomizePageSettingsParam](docs/UpdateCustomizePageSettingsParam.md)
 - [Model.UpdateCustomizePagesParam](docs/UpdateCustomizePagesParam.md)
 - [Model.UpdateEnvParam](docs/UpdateEnvParam.md)
 - [Model.UpdateIdentityProviderParam](docs/UpdateIdentityProviderParam.md)
 - [Model.UpdateNotificationMessagesParam](docs/UpdateNotificationMessagesParam.md)
 - [Model.UpdateSaasUserAttributesParam](docs/UpdateSaasUserAttributesParam.md)
 - [Model.UpdateSaasUserEmailParam](docs/UpdateSaasUserEmailParam.md)
 - [Model.UpdateSaasUserPasswordParam](docs/UpdateSaasUserPasswordParam.md)
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


