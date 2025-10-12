#!/bin/bash

# 🧩 Настройки
PROJECT="Discussions/PetFamily.Discussions.Infrastructure"
CONTEXT="DiscussionsWriteDbContext"
STARTUP="PetFamily.Web/"

timestamp=$(date +"%Y%m%d%H%M%S")

dotnet ef migrations add VolunteerRequest_$timestamp \
    --context $CONTEXT \
    --startup-project $STARTUP \
    --project $PROJECT

if [ $? -ne 0 ]; then
  echo "❌ Ошибка при создании миграции. Применение миграции отменено."
  exit 1
fi

# Применение миграции
dotnet ef database update \
    --context $CONTEXT \
    --startup-project $STARTUP \
    --project $PROJECT
