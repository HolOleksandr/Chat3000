param location string
param signalRName string
param webApiPrincipalId string

resource signalRresource 'Microsoft.SignalRService/signalR@2023-02-01' = {
  name: signalRName
  location: location
  sku: {
    name: 'Standard_S1'
    tier: 'Standard'
    capacity: 1
  }
  kind: 'SignalR'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    tls: {
      clientCertEnabled: false
    }
    features: [
      {
        flag: 'ServiceMode'
        value: 'Default'
        properties: {}
      }
    ]
    cors: {
      allowedOrigins: [
        '*'
      ]
    }
    serverless: {
      connectionTimeoutInSeconds: 30
    }
    upstream: {}
    networkACLs: {
      defaultAction: 'Deny'
      publicNetwork: {
        allow: [
          'ServerConnection'
          'ClientConnection'
          'RESTAPI'
          'Trace'
        ]
      }
      privateEndpoints: []
    }
    publicNetworkAccess: 'Enabled'
    disableLocalAuth: false
    disableAadAuth: false
  }
}

resource signalRServerRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  scope: signalRresource
  name: '420fcaa2-552c-430f-98ca-3264be4806c7'
}

resource SignalR_Add_WebApiServerRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(signalRresource.id, webApiPrincipalId, signalRServerRole.id)
  properties: {
    roleDefinitionId: signalRServerRole.id
    principalId: webApiPrincipalId
    principalType: 'ServicePrincipal'
  }
}

var ConStr = 'Endpoint=https://${signalRName}.service.signalr.net;AuthType=azure.msi;Version=1.0;'
output ConnectionStringValue string = ConStr

