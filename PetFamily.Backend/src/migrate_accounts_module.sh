#!/bin/bash

timestamp=$(date +"%Y%m%d%H%M%S")

# Генерация миграции
dotnet ef migrations add Accounts_$timestamp \
    --context AccountsDbContext \
    --startup-project PetFamily.Web/ \
    --project Accounts/PetFamily.Accounts.Infrastructure

# Применение миграции
dotnet ef database update \
    --context AccountsDbContext \
    --startup-project PetFamily.Web/ \
    --project Accounts/PetFamily.Accounts.Infrastructure

