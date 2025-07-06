#!/bin/bash

# Скрипт для создания миграции EF Core
# Использование: ./migrate.sh <имя_миграции>

if [ -z "$1" ]; then
    echo "Ошибка: Не указано имя миграции"
    echo "Использование: $0 <имя_миграции>"
    exit 1
fi

MIGRATION_NAME=$1

dotnet ef migrations add "$MIGRATION_NAME" -s PetFamily.API/ -p PetFamily.Infrastructure/ --context ApplicationWriteDbContext
