param tenantId string
param location string
param keyVaultUrl string
param keyVaultName string
param funcPrincipalId string
param webApiPrincipalId string
param connectionStringDbName string
param connectionStringDbValue string
param connectionStringStorageName string
param ConnectionStringSignalRValue string
param connectionStringStorageValue string

var uri = keyVaultUrl
var localId = '08dc2990-f13f-4ed3-9f71-a3b8003fb991'

resource chatappkeyvault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: webApiPrincipalId
        permissions: {
          secrets: [
            'get', 'list'
          ]
        }
      }
      {
        tenantId: tenantId
        objectId: funcPrincipalId
        permissions: {
          secrets: [
            'get', 'list'
          ]
        }
      }
      {
        tenantId: tenantId
        objectId: localId
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enableRbacAuthorization: false
    vaultUri: uri
    provisioningState: 'Succeeded'
    publicNetworkAccess: 'Enabled'
  }
}

resource ConnectionStrings_BlobStorageConnection 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: chatappkeyvault
  name: connectionStringDbName
  properties: {
    value: connectionStringDbValue
  }
}

resource ConnectionStrings_ProductionDbConnection 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: chatappkeyvault
  name: connectionStringStorageName
  properties: {
    value: connectionStringStorageValue
  }
}

resource ConnectionStrings_SignalRConnection 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: chatappkeyvault
  name: 'Azure--SignalR--ConnectionString'
  properties: {
    value: ConnectionStringSignalRValue
  }
}

output id string = chatappkeyvault.id

