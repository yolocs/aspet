variable "project" {
  type    = string
  default = "replaceme"
}

variable "region" {
  type    = string
  default = "us-central1"
}

variable "zone" {
  type    = string
  default = "us-central1-c"
}

provider "google" {
  project = var.project
  region  = var.region
  zone    = var.zone
}

provider "google-beta" {
  project = var.project
  region  = var.region
  zone    = var.zone
}

resource "google_sql_database_instance" "aspet_instance" {
  name             = "aspet"
  database_version = "MYSQL_5_6"

  deletion_protection = false

  settings {
    tier = "db-f1-micro"
  }
}

resource "google_sql_database" "aspet_task_db" {
  name     = "tasks"
  instance = google_sql_database_instance.aspet_instance.name
}

resource "google_sql_user" "aspet_user" {
  name     = "default-user"
  instance = google_sql_database_instance.aspet_instance.name
  host     = "%"
  password = "replaceme"
}

resource "google_service_account" "run_account" {
  account_id  = "aspet-account"
  description = "Service Account for test WP app"
}

resource "google_project_iam_member" "aspet_task_db_access" {
  project = var.project
  role    = "roles/cloudsql.client"
  member  = "serviceAccount:${google_service_account.run_account.email}"
}

resource "google_cloud_run_service" "aspet_app" {
  name     = "aspet"
  location = var.region

  template {
    metadata {
      annotations = {
        "run.googleapis.com/cloudsql-instances" = google_sql_database_instance.aspet_instance.connection_name
      }
    }

    spec {
      service_account_name = google_service_account.run_account.email

      containers {
        image = "gcr.io/cshou-playground/aspet:v6"
        env {
          name  = "DB_NAME"
          value = google_sql_database.aspet_task_db.name
        }
        env {
          name  = "DB_USER"
          value = google_sql_user.aspet_user.name
        }
        env {
          name  = "DB_PASSWORD"
          value = google_sql_user.aspet_user.password
        }
        env {
          name  = "INSTANCE_CONNECTION_NAME"
          value = "/cloudsql/${google_sql_database_instance.aspet_instance.connection_name}"
        }
        env {
          name  = "FIREBASE_API_KEY"
          value = data.google_firebase_web_app_config.aspet_app_config.api_key
        }
      }
    }
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
}

resource "google_firebase_project" "default" {
  provider = google-beta
}

resource "google_firebase_web_app" "aspet_app" {
  provider     = google-beta
  project      = var.project
  display_name = "aspet app"

  depends_on = [google_firebase_project.default]
}

data "google_firebase_web_app_config" "aspet_app_config" {
  provider   = google-beta
  web_app_id = google_firebase_web_app.aspet_app.app_id
}
