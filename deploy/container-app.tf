resource "azurerm_log_analytics_workspace" "log_analytics" {
  name                = local.unique_name
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_container_app_environment" "environment" {
  name                       = local.unique_name
  location                   = data.azurerm_resource_group.rg.location
  resource_group_name        = data.azurerm_resource_group.rg.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log_analytics.id
}

resource "azurerm_container_app" "web" {
  name                         = "${local.unique_name}-web"
  container_app_environment_id = azurerm_container_app_environment.environment.id
  resource_group_name          = data.azurerm_resource_group.rg.name
  revision_mode                = "Single"
  
  ingress {
    external_enabled = true
    allow_insecure_connections = false
    target_port = 80
    traffic_weight {
      latest_revision = true
      percentage = 100
    }
  }

  secret {
    name = "servicebus"
    value = azurerm_servicebus_namespace_authorization_rule.web.primary_connection_string
  }

  template {   
    container {
      name   = "web"
      image  = var.web_tag
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name = "ConnectionStrings__ServiceBus"
        secret_name = "servicebus"
      }
    }
  }
}

resource "azurerm_container_app" "worker" {
  name                         = "${local.unique_name}-worker"
  container_app_environment_id = azurerm_container_app_environment.environment.id
  resource_group_name          = data.azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "worker"
      image  = var.worker_tag
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}