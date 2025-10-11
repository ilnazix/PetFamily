#!/bin/bash

timestamp=$(date +"%Y%m%d%H%M%S")

# Генерация миграции
dotnet ef migrations add VolunteerRequest_$timestamp \
    --context VolunteerRequestsWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project VolunteerRequest/PetFamily.VolunteerRequest.Infrastructure

# Применение миграции
dotnet ef database update \
    --context VolunteerRequestsWriteDbContext \
    --startup-project PetFamily.Web/ \
    --project VolunteerRequest/PetFamily.VolunteerRequest.Infrastructure

