variable "app_replicas" {
  description = "Az app Pod-ok száma"
  type        = number
  default     = 2
}

variable "app_image" {
  description = "A Docker image neve"
  type        = string
  default     = "gaborkozak/equipment-tracker:latest"
}