# authapi.Model.BasicInfo

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**DomainName** | **string** | Domain Name | 
**IsDnsValidated** | **bool** | DNS Record Verification Results | 
**DomainSetupStatus** | **string** | Domain setup status. Pending means validation or configuration is in progress; action_required means user action is needed; completed means configuration has been accepted. | [optional] 
**DomainSetupReasonCode** | **string** | Reason domain setup is incomplete. Omitted when no known reason is available. DNS validation may still be pending while records propagate. Failed or timed-out certificate requests require resubmitting the domain settings after correcting DNS. | [optional] 
**CertificateDnsRecord** | [**DnsRecord**](DnsRecord.md) |  | 
**CloudFrontDnsRecord** | [**DnsRecord**](DnsRecord.md) |  | 
**DkimDnsRecords** | [**List&lt;DnsRecord&gt;**](DnsRecord.md) | DKIM DNS Records | 
**DefaultDomainName** | **string** | Default Domain Name | 
**FromEmailAddress** | **string** | Sender Email for Authentication Email | 
**ReplyEmailAddress** | **string** | Reply-from email address of authentication email | 
**IsSesSandboxGranted** | **bool** | SES sandbox release and Cognito SES configuration results | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

