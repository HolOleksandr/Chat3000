@secure()
param jwtKey string
param location string
param jwtIssuer string
param jwtAudience string
param keyVaultName string
param functionAppName string
param appServicePlanId string
param connectionStringStorageValue string

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlanId
    clientAffinityEnabled: true
  }
}

resource apiAppSettings 'Microsoft.Web/sites/config@2022-09-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: functionApp
  properties: {
    AzureWebJobsStorage: connectionStringStorageValue
    FUNCTIONS_EXTENSION_VERSION: '~4'
    FUNCTIONS_WORKER_RUNTIME: 'dotnet'
    keyVaultName: keyVaultName
    WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: connectionStringStorageValue
    WEBSITE_CONTENTSHARE: functionAppName
    JwtKey: jwtKey
    JwtIssuer: jwtIssuer
    JwtAudience: jwtAudience
  }
}

output Url string = 'https://${functionApp.name}.azurewebsites.net/api/TextFileConverter'
output PrincipalId string = functionApp.identity.principalId
