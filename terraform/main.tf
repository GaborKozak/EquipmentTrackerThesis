terraform {
  required_providers {
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.0"
    }
  }
}

provider "kubernetes" {
  config_path    = "~/.kube/config"
  config_context = "minikube"
}

provider "helm" {
  kubernetes {
    config_path    = "~/.kube/config"
    config_context = "minikube"
  }
}

resource "helm_release" "equipment_tracker" {
  name      = "equipment-tracker"
  chart     = "${path.module}/../helm/equipment-tracker"

  set {
    name  = "app.replicas"
    value = var.app_replicas
  }

  set {
    name  = "app.image"
    value = var.app_image
  }
}