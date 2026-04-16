#!/bin/bash
set -e

SA_PASSWORD=$1

if [ -z "$SA_PASSWORD" ]; then
    echo "Usage: ./installation.sh <SA_PASSWORD>"
    exit 1
fi

# Ensure config files are files, not directories
for config in loki-config.yaml promtail-config.yaml prometheus-config.yaml; do
    if [ -d "$config" ]; then
        echo "WARNING: $config is a directory, removing..."
        rm -rf "$config"
    fi
done

# Database services list with corresponding SQL script directories
declare -a DB_SERVICES=("customers_db" "products_db" "baskets_db" "orders_db")

declare -A DB_SCRIPT_DIRS
DB_SCRIPT_DIRS["customers_db"]="Database/Customers"
DB_SCRIPT_DIRS["products_db"]="Database/Products"
DB_SCRIPT_DIRS["baskets_db"]="Database/Baskets"
DB_SCRIPT_DIRS["orders_db"]="Database/Orders"

# Run database containers
echo ">>> Starting database containers..."
docker compose up -d "${DB_SERVICES[@]}"

# Wait until all databases are healthy
echo ">>> Waiting for databases to be ready..."
for service in "${DB_SERVICES[@]}"; do
    echo "  Waiting for $service..."
    attempt=1
    max_attempts=60
    while [ $attempt -le $max_attempts ]; do
        container_id=$(docker compose ps -q "$service")
        status=$(docker inspect "$container_id" --format='{{.State.Health.Status}}' 2>/dev/null)
        if [ "$status" = "healthy" ]; then
            echo "  $service is healthy"
            break
        fi
        if [ $attempt -eq $max_attempts ]; then
            echo "ERROR: $service did not become healthy after $max_attempts attempts"
            exit 1
        fi
        echo "    Attempt $attempt/$max_attempts - status: $status"
        sleep 5
        attempt=$((attempt + 1))
    done
done

# For each database run SQL scripts
echo ">>> Running database initialization scripts..."
for service in "${DB_SERVICES[@]}"; do
    script_dir="${DB_SCRIPT_DIRS[$service]}"
    echo "  Initializing $service from $script_dir..."
    for script in $(ls "$script_dir"/*.sql | sort); do
        script_name=$(basename "$script")
        echo "    Running $script_name..."
        docker compose exec -T "$service" \
            /opt/mssql-tools18/bin/sqlcmd \
            -S localhost -U sa -P "$SA_PASSWORD" \
            -C -b \
            -i "/scripts/$script_name"
        echo "    $script_name done"
    done
done

# Running all services
echo ">>> Starting all services..."
docker compose up -d

echo ">>> Installation complete!"
