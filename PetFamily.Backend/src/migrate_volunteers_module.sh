#!/bin/bash

timestamp=$(date +"%Y%m%d%H%M%S")

# Генерация миграции
dotnet ef migrations add Volunteers_$timestamp \
    --context VolunteersWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project Volunteers/PetFamily.Volunteers.Infrastructure

# Применение миграции
dotnet ef database update \
    --context VolunteersWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project Volunteers/PetFamily.Volunteers.Infrastructure

