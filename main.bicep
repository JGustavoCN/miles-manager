@secure()
param vulnerabilityAssessments_Default_storageContainerPath string
param servers_sqlserver_miles_jgustavocn_name string
param sites_miles_manager_app_jgustavocn_name string
param serverfarms_ASP_rgmilesmanager_98ef_name string

resource servers_sqlserver_miles_jgustavocn_name_resource 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: servers_sqlserver_miles_jgustavocn_name
  location: 'eastus2'
  kind: 'v12.0'
  properties: {
    administratorLogin: 'milesadmin'
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'User'
      login: 'josegustavocorreianascimento@gmail.com'
      sid: '8b8b33b6-0ec1-4f7f-8c0c-1a9135cfcd94'
      tenantId: 'e82b754f-e097-42be-9834-f263bd97af01'
      azureADOnlyAuthentication: false
    }
    restrictOutboundNetworkAccess: 'Disabled'
  }
}

resource serverfarms_ASP_rgmilesmanager_98ef_name_resource 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: serverfarms_ASP_rgmilesmanager_98ef_name
  location: 'East US'
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 0
  }
  kind: 'app'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: false
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
    asyncScalingEnabled: false
  }
}

resource servers_sqlserver_miles_jgustavocn_name_ActiveDirectory 'Microsoft.Sql/servers/administrators@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: 'josegustavocorreianascimento@gmail.com'
    sid: '8b8b33b6-0ec1-4f7f-8c0c-1a9135cfcd94'
    tenantId: 'e82b754f-e097-42be-9834-f263bd97af01'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/advancedThreatProtectionSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_CreateIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_DbParameterization 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_DefragmentIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_DropIndex 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'DropIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_ForceLastGoodPlan 'Microsoft.Sql/servers/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: 'Enabled'
  }
}

resource Microsoft_Sql_servers_auditingPolicies_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/auditingPolicies@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  location: 'East US 2'
  properties: {
    auditingState: 'Disabled'
  }
}

resource Microsoft_Sql_servers_auditingSettings_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/auditingSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'default'
  properties: {
    retentionDays: 0
    auditActionsAndGroups: []
    isStorageSecondaryKeyInUse: false
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource Microsoft_Sql_servers_azureADOnlyAuthentications_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/azureADOnlyAuthentications@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    azureADOnlyAuthentication: false
  }
}

resource Microsoft_Sql_servers_connectionPolicies_servers_sqlserver_miles_jgustavocn_name_default 'Microsoft.Sql/servers/connectionPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'default'
  location: 'eastus2'
  properties: {
    connectionType: 'Default'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'sqldb-miles-manager'
  location: 'eastus2'
  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 2
  }
  kind: 'v12.0,user,vcore,serverless,freelimit'
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    autoPauseDelay: 60
    requestedBackupStorageRedundancy: 'Local'
    minCapacity: json('0.5')
    maintenanceConfigurationId: '/subscriptions/fec66b1e-7ea5-4a48-b89a-0c4e25f7c78b/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
    isLedgerOn: false
    useFreeLimit: true
    freeLimitExhaustionBehavior: 'AutoPause'
    availabilityZone: 'NoPreference'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingPolicies_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  location: 'East US 2'
  properties: {
    auditingState: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingSettings_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/auditingSettings@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    retentionDays: 0
    isAzureMonitorTargetEnabled: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_extendedAuditingSettings_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    retentionDays: 0
    isAzureMonitorTargetEnabled: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_geoBackupPolicies_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/geoBackupPolicies@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_master_Current 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Current'
  properties: {}
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_securityAlertPolicies_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/securityAlertPolicies@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    state: 'Disabled'
    disabledAlerts: [
      ''
    ]
    emailAddresses: [
      ''
    ]
    emailAccountAdmins: false
    retentionDays: 0
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_transparentDataEncryption_servers_sqlserver_miles_jgustavocn_name_master_Current 'Microsoft.Sql/servers/databases/transparentDataEncryption@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Current'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_vulnerabilityAssessments_servers_sqlserver_miles_jgustavocn_name_master_Default 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2024-05-01-preview' = {
  name: '${servers_sqlserver_miles_jgustavocn_name}/master/Default'
  properties: {
    recurringScans: {
      isEnabled: false
      emailSubscriptionAdmins: true
    }
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_devOpsAuditingSettings_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/devOpsAuditingSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_current 'Microsoft.Sql/servers/encryptionProtector@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'current'
  kind: 'servicemanaged'
  properties: {
    serverKeyName: 'ServiceManaged'
    serverKeyType: 'ServiceManaged'
    autoRotationEnabled: false
  }
}

resource Microsoft_Sql_servers_extendedAuditingSettings_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/extendedAuditingSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'default'
  properties: {
    retentionDays: 0
    auditActionsAndGroups: []
    isStorageSecondaryKeyInUse: false
    isAzureMonitorTargetEnabled: false
    isManagedIdentityInUse: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_ClientIp_2026_2_2_17_23_34 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'ClientIp-2026-2-2_17-23-34'
  properties: {
    startIpAddress: '191.7.97.202'
    endIpAddress: '191.7.97.202'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_ServiceManaged 'Microsoft.Sql/servers/keys@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'ServiceManaged'
  kind: 'servicemanaged'
  properties: {
    serverKeyType: 'ServiceManaged'
  }
}

resource Microsoft_Sql_servers_securityAlertPolicies_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/securityAlertPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    state: 'Disabled'
    disabledAlerts: [
      ''
    ]
    emailAddresses: [
      ''
    ]
    emailAccountAdmins: false
    retentionDays: 0
  }
}

resource Microsoft_Sql_servers_sqlVulnerabilityAssessments_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/sqlVulnerabilityAssessments@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

resource Microsoft_Sql_servers_vulnerabilityAssessments_servers_sqlserver_miles_jgustavocn_name_Default 'Microsoft.Sql/servers/vulnerabilityAssessments@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_resource
  name: 'Default'
  properties: {
    recurringScans: {
      isEnabled: false
      emailSubscriptionAdmins: true
    }
    storageContainerPath: vulnerabilityAssessments_Default_storageContainerPath
  }
}

resource sites_miles_manager_app_jgustavocn_name_resource 'Microsoft.Web/sites@2024-11-01' = {
  name: sites_miles_manager_app_jgustavocn_name
  location: 'East US'
  kind: 'app'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${sites_miles_manager_app_jgustavocn_name}-acbhhhavcgchgsgk.eastus-01.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${sites_miles_manager_app_jgustavocn_name}-acbhhhavcgchgsgk.scm.eastus-01.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_ASP_rgmilesmanager_98ef_name_resource.id
    reserved: false
    isXenon: false
    hyperV: false
    dnsConfiguration: {}
    outboundVnetRouting: {
      allTraffic: false
      applicationTraffic: false
      contentShareTraffic: false
      imagePullTraffic: false
      backupRestoreTraffic: false
    }
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 0
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientAffinityProxyEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    ipMode: 'IPv4'
    customDomainVerificationId: '0328BA5780CD1FEFD1AA9AF9717A922F7F5782FFCF5B2DBB9A567A28BF7B5A41'
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    endToEndEncryptionEnabled: false
    redundancyMode: 'None'
    publicNetworkAccess: 'Enabled'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
    autoGeneratedDomainNameLabelScope: 'TenantReuse'
  }
}

resource sites_miles_manager_app_jgustavocn_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: sites_miles_manager_app_jgustavocn_name_resource
  name: 'ftp'
  location: 'East US'
  properties: {
    allow: true
  }
}

resource sites_miles_manager_app_jgustavocn_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2024-11-01' = {
  parent: sites_miles_manager_app_jgustavocn_name_resource
  name: 'scm'
  location: 'East US'
  properties: {
    allow: true
  }
}

resource sites_miles_manager_app_jgustavocn_name_web 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: sites_miles_manager_app_jgustavocn_name_resource
  name: 'web'
  location: 'East US'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
      'hostingstart.html'
    ]
    netFrameworkVersion: 'v9.0'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$miles-manager-app-jgustavocn'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    publicNetworkAccess: 'Enabled'
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 2147483647
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.2'
    ftpsState: 'FtpsOnly'
    preWarmedInstanceCount: 0
    elasticWebAppScaleLimit: 0
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 0
    azureStorageAccounts: {}
    http20ProxyFlag: 0
  }
}

resource sites_miles_manager_app_jgustavocn_name_c623cf2fb8cb4eeea309947ff98dcd66 'Microsoft.Web/sites/deployments@2024-11-01' = {
  parent: sites_miles_manager_app_jgustavocn_name_resource
  name: 'c623cf2fb8cb4eeea309947ff98dcd66'
  location: 'East US'
  properties: {
    status: 4
    author_email: 'N/A'
    author: 'N/A'
    deployer: 'GITHUB_ZIP_DEPLOY'
    message: '{"type":"deployment","sha":"ea86be71329e1435e21444b7ab9f4c8a08622716","repoName":"JGustavoCN/miles-manager","actor":"JGustavoCN","slotName":"production","commitMessage":"feat: preparao para o deploy"}'
    start_time: '2026-02-03T03:25:57.8949685Z'
    end_time: '2026-02-03T03:26:01.5047269Z'
    active: true
  }
}

resource sites_miles_manager_app_jgustavocn_name_sites_miles_manager_app_jgustavocn_name_acbhhhavcgchgsgk_eastus_01_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2024-11-01' = {
  parent: sites_miles_manager_app_jgustavocn_name_resource
  name: '${sites_miles_manager_app_jgustavocn_name}-acbhhhavcgchgsgk.eastus-01.azurewebsites.net'
  location: 'East US'
  properties: {
    siteName: 'miles-manager-app-jgustavocn'
    hostNameType: 'Verified'
  }
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/advancedThreatProtectionSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_CreateIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'CreateIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_DbParameterization 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'DbParameterization'
  properties: {
    autoExecuteValue: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_DefragmentIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'DefragmentIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_DropIndex 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'DropIndex'
  properties: {
    autoExecuteValue: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_ForceLastGoodPlan 'Microsoft.Sql/servers/databases/advisors@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'ForceLastGoodPlan'
  properties: {
    autoExecuteValue: 'Enabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingPolicies_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/auditingPolicies@2014-04-01' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Default'
  location: 'East US 2'
  properties: {
    auditingState: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_auditingSettings_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/auditingSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'default'
  properties: {
    retentionDays: 0
    isAzureMonitorTargetEnabled: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_backupLongTermRetentionPolicies_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_default 'Microsoft.Sql/servers/databases/backupLongTermRetentionPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'default'
  properties: {
    weeklyRetention: 'PT0S'
    monthlyRetention: 'PT0S'
    yearlyRetention: 'PT0S'
    weekOfYear: 0
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_backupShortTermRetentionPolicies_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_default 'Microsoft.Sql/servers/databases/backupShortTermRetentionPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'default'
  properties: {
    retentionDays: 7
    diffBackupIntervalInHours: 12
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_extendedAuditingSettings_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/extendedAuditingSettings@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'default'
  properties: {
    retentionDays: 0
    isAzureMonitorTargetEnabled: false
    state: 'Disabled'
    storageAccountSubscriptionId: '00000000-0000-0000-0000-000000000000'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_geoBackupPolicies_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/geoBackupPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Current 'Microsoft.Sql/servers/databases/ledgerDigestUploads@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Current'
  properties: {}
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_securityAlertPolicies_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/securityAlertPolicies@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Default'
  properties: {
    state: 'Disabled'
    disabledAlerts: [
      ''
    ]
    emailAddresses: [
      ''
    ]
    emailAccountAdmins: false
    retentionDays: 0
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_transparentDataEncryption_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Current 'Microsoft.Sql/servers/databases/transparentDataEncryption@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Current'
  properties: {
    state: 'Enabled'
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}

resource Microsoft_Sql_servers_databases_vulnerabilityAssessments_servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager_Default 'Microsoft.Sql/servers/databases/vulnerabilityAssessments@2024-05-01-preview' = {
  parent: servers_sqlserver_miles_jgustavocn_name_sqldb_miles_manager
  name: 'Default'
  properties: {
    recurringScans: {
      isEnabled: false
      emailSubscriptionAdmins: true
    }
  }
  dependsOn: [
    servers_sqlserver_miles_jgustavocn_name_resource
  ]
}
