param location string
param sqlDbName string
param sqlServerName string
@secure()
param SqlServerAdminLogin string
@secure()
param SqlServerAdminPass string

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: SqlServerAdminLogin
    administratorLoginPassword: SqlServerAdminPass
    version: '12.0'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-11-01' = {
  name: sqlDbName
  location: location
  parent: sqlServer
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

resource SqlAllowAzureIps 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  name: 'AlowAzureIps'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource SqlAllowAdminsIps 'Microsoft.Sql/servers/firewallRules@2021-11-01' = {
  name: 'SqlAllowAdminsIps'
  parent: sqlServer
  properties: {
    startIpAddress: '93.73.163.20'
    endIpAddress: '93.73.163.20'
  }
}
