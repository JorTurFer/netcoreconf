
locals {
  unique_name = "netcoreconf"
}

data "azurerm_resource_group" "rg" {
  name = local.unique_name
}