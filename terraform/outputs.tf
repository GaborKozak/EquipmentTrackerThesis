output "release_name" {
  description = "A Helm release neve"
  value       = helm_release.equipment_tracker.name
}

output "release_status" {
  description = "A Helm release státusza"
  value       = helm_release.equipment_tracker.status
}