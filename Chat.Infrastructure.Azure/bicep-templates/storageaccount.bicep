param location string
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

var key = storageAccount.listKeys().keys[0].value

output storageName string = storageAccount.name
output key string = key
output ConnectionStringValue string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${key};EndpointSuffix=core.windows.net'

