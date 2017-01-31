# Tenant-Level RBAC #


## Overview ##

Role Based Access Control (RBAC) has previously been applicable to three scopes: a subscription, a resource group, and a resource. 

With tenant-level RBAC, you are able to assign users to roles at the tenant-level ("/") which means that is across all subscriptions. 

## Tenant Admin ##

In order to assign roles at the tenant-level you need to be an Azure AD Administrator.


## Role definitions ##

Role definitions at the tenant level scope ("/") are inherited by all ARM subscriptions, resource groups, and resources within the tenant

## Role assignment ##

Currently you have to use the ARM REST API directly to assign roles at the tenant-level. 

NOTE: armclient is a useful tool to make ARM REST API calls. It is available on github - [ARMClient on github](https://github.com/projectkudu/ARMClient) 


The role assignment is composed of the following two calls. 


### Elevate access
The first call you have to make is to the elevateAccess action. 

	POST https://management.azure.com/providers/Microsoft.Authorization/elevateAccess?api-version=2015-07-01

### Assign role

Then you can assign a role, like this:

	PUT  https://management.azure.com/providers/Microsoft.Authorization/roleAssignments/64736CA0-56D7-4A94-A551-973C2FE7888B?api-version=2015-07-01
	## Request Body
	{ 
		"properties“ : {
     		"roleDefinitionId“ : "providers/Microsoft.Authorization/roleDefinitions/18d7d88d-d35e-4fb5-a5c3-7773c20a72d9",
     		"principalId“ : "cbc5e050-d7cd-4310-813b-4870be8ef5bb",
     		"scope“ : "/“
	  },
	  	"id“ : "providers/Microsoft.Authorization/roleAssignments/64736CA0-56D7-4A94-A551-973C2FE7888B",
		"type“ : "Microsoft.Authorization/roleAssignments",
		"name“ : "64736CA0-56D7-4A94-A551-973C2FE7888B"
	}


You need to create a new GUID to use for the role assignment Id and then replace it in the URL for the call, the ID in the request body, and as the name (if you want). 

You can retrieve the role definitions with the following PowerShell command which will give you the Id. 

	Get-AzureRmRoleDefinition

NOTE: Using armclient, you can put the request body into a file and refer to it on the command line using "@"




