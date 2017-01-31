
# Create a role for Cloud Service contributors based on Classic VM Contributor

$role = Get-AzureRmRoleDefinition "Classic Virtual Machine Contributor"
$role.Id = $null
$role.Name = "Cloud Services Contributor"
$role.Description = "Can create and delete Cloud Services"
$role.Actions.Remove("Microsoft.ClassicCompute/virtualMachines/*")
$role.Actions.Remove("Microsoft.ClassicNetwork/networkSecurityGroups/join/action")
$role.AssignableScopes.Clear()
$role.AssignableScopes.Add("/subscriptions/####")
New-AzureRmRoleDefinition -Role $role

New-AzureRmRoleAssignment -ObjectId #### -ResourceGroupName cstest-rg -RoleDefinitionName "Cloud Services Contributor"
 


