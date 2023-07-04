param jwtKey string
param apiname string
param location string
param jwtAudience string
param keyVaultName string
param appServicePlanId string

 var webApiUrl  = 'https://${webAppApi.name}.azurewebsites.net/'
 
resource webAppApi 'Microsoft.Web/sites@2022-09-01' = {
  name: apiname
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'app'
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlanId
    reserved: true
    siteConfig: {
      netFrameworkVersion: 'v6.0'
    }
    publicNetworkAccess: 'Enabled'
  }
}

resource apiAppSettings 'Microsoft.Web/sites/config@2022-09-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: webAppApi
  properties: {
    KeyVaultName: keyVaultName
    ProdJwt__Key: jwtKey
    ProdJwt__Issuer: webApiUrl
    ProdJwt__Audience: jwtAudience
  }
}

output webApiUrl string = webApiUrl
output webApiUrlCORS string = 'https://${webAppApi.name}.azurewebsites.net'
output apiPrincipalId string = webAppApi.identity.principalId



