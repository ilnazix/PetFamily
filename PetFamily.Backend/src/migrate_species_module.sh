#!/bin/bash

timestamp=$(date +"%Y%m%d%H%M%S")

# Генерация миграции
dotnet ef migrations add Species_$timestamp \
    --context SpeciesWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project Species/PetFamily.Species.Infrastructure

# Применение миграции
dotnet ef database update \
    --context SpeciesWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project Species/PetFamily.Species.Infrastructure
