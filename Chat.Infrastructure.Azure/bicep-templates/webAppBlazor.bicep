param location string
param webApiUrl string
param azureFuncUrl string
param blazorappname string
param appServicePlanId string

resource webAppBlazor 'Microsoft.Web/sites@2022-09-01' = {
  name: blazorappname
  location: location
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

resource blazorAppSettings 'Microsoft.Web/sites/config@2022-09-01' = {
  name: 'appsettings'
  kind: 'string'
  parent: webAppBlazor
  properties: {
    ChatApi: webApiUrl
    FileTransformAzureFuncUrl: azureFuncUrl
  }
}
