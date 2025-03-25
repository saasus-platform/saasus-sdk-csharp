# authapi.Model.Tenant

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Name** | **string** | tenant name | 
**Attributes** | **Dictionary&lt;string, Object&gt;** | attribute info | 
**BackOfficeStaffEmail** | **string** | administrative staff email address | 
**NextPlanId** | **string** |  | [optional] 
**UsingNextPlanFrom** | **int** | Next billing plan start time (When using stripe, you can create a subscription that starts at the beginning of the current month by specifying 00:00 (UTC) at the beginning of the current month. Ex. 1672531200 for January 2023.)  | [optional] 
**NextPlanTaxRateId** | **string** |  | [optional] 
**ProrationBehavior** | **ProrationBehavior** |  | [optional] 
**DeleteUsage** | **bool** | If you have a stripe linkage,  you can set whether to delete pay-as-you-go items when changing plans. When you change plan, you can remove all pay-as-you-go items included in your current subscription to stop being billed based on pay-as-you-go items. The recorded usage is cleared immediately. Since it cannot be restored, please note that plan change reservations with delete_usage set to true cannot be canceled.  | [optional] 
**PlanHistories** | [**List&lt;PlanHistory&gt;**](PlanHistory.md) | Plan History | 
**Id** | **string** |  | 
**PlanId** | **string** |  | [optional] 
**BillingInfo** | [**BillingInfo**](BillingInfo.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

