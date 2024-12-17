# authapi.Model.UserAvailableTenant

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** |  | 
**Name** | **string** | Tenant Name | 
**CompletedSignUp** | **bool** |  | 
**Envs** | [**List&lt;UserAvailableEnv&gt;**](UserAvailableEnv.md) | environmental info, role info | 
**UserAttribute** | **Dictionary&lt;string, Object&gt;** | user additional attributes | 
**BackOfficeStaffEmail** | **string** | back office contact email | 
**PlanId** | **string** |  | [optional] 
**IsPaid** | **bool** | tenant payment status ※ Currently, it is returned only when stripe is linked.  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

