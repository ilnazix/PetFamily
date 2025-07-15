#!/bin/bash

# Скрипт для применения миграций EF Core
# Использование: ./apply_migrations.sh

echo "Применение миграций для ApplicationWriteDbContext..."

dotnet ef database update \
    --startup-project PetFamily.API/ \
    --project PetFamily.Infrastructure/ \
    --context ApplicationWriteDbContext

if [ $? -eq 0 ]; then
    echo "Миграции успешно применены"
else
    echo "Ошибка при применении миграций"
    exit 1
fi

