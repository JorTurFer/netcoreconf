terraform {
  backend "azurerm" {
    key = "keda.tfstate"
  }

  required_version = ">= 1.5.6"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.10.0"
    }

  }
}

provider "azurerm" {
  features {}
  skip_provider_registration = true
}