resource "azurerm_servicebus_namespace" "servicebus" {
  name                = local.unique_name
  location            = data.azurerm_resource_group.rg.location
  resource_group_name = data.azurerm_resource_group.rg.name
  sku                 = "Basic"

  tags = {
    source = "terraform"
  }
}

resource "azurerm_servicebus_queue" "queue" {
  name         = "orders"
  namespace_id = azurerm_servicebus_namespace.servicebus.id

  partitioning_enabled = true
}

resource "azurerm_servicebus_namespace_authorization_rule" "autoscaler" {
  name         = "autoscaler"
  namespace_id = azurerm_servicebus_namespace.servicebus.id

  listen = true
  send   = true
  manage = false
}

resource "azurerm_servicebus_namespace_authorization_rule" "worker" {
  name         = "worker"
  namespace_id = azurerm_servicebus_namespace.servicebus.id

  listen = true
  send   = true
  manage = false
}

resource "azurerm_servicebus_namespace_authorization_rule" "web" {
  name         = "web"
  namespace_id = azurerm_servicebus_namespace.servicebus.id

  listen = true
  send   = true
  manage = false
}
