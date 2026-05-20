#!/bin/bash
echo "Nuking the database and Docker volumes..."
docker compose --profile db down -v
docker compose --profile db up -d

sleep 3

echo "Scrapping old migrations..."
rm -rf Migrations

echo "Generating new initial migration..."
dotnet ef migrations add InitialCreate

echo "Updating database..."
dotnet ef database update

echo "Done! Back to work."
