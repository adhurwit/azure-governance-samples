# Azure Governance samples #


## Tenant-Level RBAC ##

Role Based Access Control (RBAC) has previously been applicable to three scopes: a subscription, a resource group, and a resource. 

With tenant-level RBAC, you are able to assign users to roles at the tenant-level ("/") which means that is across all subscriptions. 

Learn more here - [Tenant-Level RBAC instructions](tenant-level-rbac.md)


## Activity Log Monitoring ##

This solution will provide a monitoring mechanism for Azure Activity Logs. This currently requires two processes. 

Here is a diagram of the solution: 



![Activity Log Monitoring Processes][1]


Learn more here - [Activity Log Monitoring](activity-log-monitoring.md)




## Custom Role for Cloud Services Contributor ##

Many customers are in a situation where they need to support Cloud Services even though they have made the move to Azure Resource Manager. 

This has created the need for a custom role for Cloud Services Contributor. The role is based on the resource provider for the classic services, specifically Microsoft.ClassicCompute. The resource type for Cloud Services is Microsoft.ClassicCompute/domainNames

This repo contains a PowerShell script that will create the custom role. For more information on creating custom roles, see [Manage Role-Based Access Control with Azure PowerShell](https://docs.microsoft.com/en-us/azure/active-directory/role-based-access-control-manage-access-powershell)


<!--Image references-->
[1]: ./alog-monitor-processes.png