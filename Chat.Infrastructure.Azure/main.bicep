param location string = resourceGroup().location
param tenantId string = subscription().tenantId

@secure()
param jwtKey string
param apiname string
param sqlDbName string
param signalRName string
param keyVaultName string
param SqlAdminLogin string
param sqlServerName string
param blazorappname string
param functionAppName string
param storageAccountName string
param connectionStringDbName string
param connectionStringStorageName string

@secure()
param SqlAdminPass string

var keyvaulturl ='https://${keyVaultName}.vault.azure.net/'
var sqlserverurl = '${sqlServerName}.database.windows.net'
var blazorServerUrl = 'https://${blazorappname}.azurewebsites.net/'
var connectionStringDbValue = 'Server=tcp:${sqlserverurl},1433;Initial Catalog=${sqlDbName};Persist Security Info=False;User ID=${SqlAdminLogin};Password=${SqlAdminPass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

module storageAccountModule 'bicep-templates/storageaccount.bicep' = {
  name: 'storageAccountModule'
  params: {
    location: location
    storageAccountName: storageAccountName
  }
}
output storageAccountNameOutput string = storageAccountModule.outputs.storageName

module sqlServerModule 'bicep-templates/sqlserver.bicep' = {
  name: 'sqlServerModule'
  params: {
    location: location
    sqlDbName: sqlDbName
    sqlServerName: sqlServerName
    SqlServerAdminPass: SqlAdminPass
    SqlServerAdminLogin: SqlAdminLogin
  }
}

module servicePlanModule 'bicep-templates/appserviceplan.bicep' = {
  name: 'servicePlanModule'
  params: {
    location: location
  }
}

module webApiModule 'bicep-templates/webAppApi.bicep' = {
  name: 'webApiModule'
  params: {
    jwtKey: jwtKey
    apiname: apiname
    location: location
    jwtAudience: blazorServerUrl
    keyVaultName: keyVaultName
    appServicePlanId: servicePlanModule.outputs.id
  }
  dependsOn:[
    servicePlanModule
  ]
}

module signalRModule 'bicep-templates/signalr.bicep' = {
  name: 'signalRModule'
  params: {
    location: location
    signalRName: signalRName
    webApiPrincipalId: webApiModule.outputs.apiPrincipalId
  }
  dependsOn:[
    webApiModule
  ]
}

module keyVaultModule 'bicep-templates/keyvault.bicep' = {
  name: 'keyVaultModule'
  params: {
    location: location
    tenantId: tenantId
    keyVaultUrl: keyvaulturl
    keyVaultName: keyVaultName
    funcPrincipalId: azureFuncModule.outputs.PrincipalId
    webApiPrincipalId: webApiModule.outputs.apiPrincipalId
    connectionStringDbName: connectionStringDbName
    connectionStringDbValue: connectionStringDbValue
    connectionStringStorageName: connectionStringStorageName
    connectionStringStorageValue: storageAccountModule.outputs.ConnectionStringValue
    ConnectionStringSignalRValue: signalRModule.outputs.ConnectionStringValue
  }
  dependsOn:[
    webApiModule
    signalRModule
    azureFuncModule
    sqlServerModule
    storageAccountModule
  ]
}

module azureFuncModule 'bicep-templates/azurefunction.bicep' = {
  name: 'azureFuncModule'
  params: {
    jwtKey: jwtKey
    location: location
    jwtIssuer: webApiModule.outputs.webApiUrl
    jwtAudience: blazorServerUrl
    keyVaultName: keyVaultName
    functionAppName: functionAppName
    appServicePlanId: servicePlanModule.outputs.id
    connectionStringStorageValue: storageAccountModule.outputs.ConnectionStringValue
  }
}

module blazorAppModule 'bicep-templates/webAppBlazor.bicep' = {
  name: 'blazorAppModule'
  params: {
    location: location
    webApiUrl: webApiModule.outputs.webApiUrl
    azureFuncUrl: azureFuncModule.outputs.Url
    blazorappname: blazorappname
    appServicePlanId: servicePlanModule.outputs.id
  }
  dependsOn:[
    webApiModule
    azureFuncModule
  ]
}
