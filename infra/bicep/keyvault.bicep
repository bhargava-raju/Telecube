// Key Vault + a user-assigned managed identity that every Telecube service uses to read
// secrets/connection strings — see Telecube.BuildingBlocks.Web/KeyVaultExtensions.cs for the
// corresponding app-side wiring.

@description('Azure region for all resources in this module.')
param location string = resourceGroup().location

@description('Base name used to derive resource names (e.g. "telecube").')
param namePrefix string

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${namePrefix}-identity'
  location: location
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: '${namePrefix}-kv'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    // TODO: decide soft-delete/purge-protection settings for this environment.
  }
}

// TODO: role assignment granting managedIdentity "Key Vault Secrets User" on keyVault
// (least-privilege — see the design doc's Security section).

output managedIdentityId string = managedIdentity.id
output managedIdentityClientId string = managedIdentity.properties.clientId
output keyVaultUri string = keyVault.properties.vaultUri
