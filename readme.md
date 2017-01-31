# Azure Governance samples #


## Custom Role for Cloud Service Contributor ##

Many customers are in a situation where they need to support Cloud Service even though they have made the move to Azure Resource Manager. 

This has created the need for a custom role for Cloud Service Contributor. The role is based on the resource provider for the classic services, specifically Microsoft.ClassicCompute. The resource type for Cloud Service is Microsoft.ClassicCompute/domainNames

This repo contains a PowerShell script that will create the custom role. For more information on creating custom roles, see [Manage Role-Based Access Control with Azure PowerShell](https://docs.microsoft.com/en-us/azure/active-directory/role-based-access-control-manage-access-powershell)