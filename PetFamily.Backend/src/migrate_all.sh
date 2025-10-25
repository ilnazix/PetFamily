#!/bin/bash

echo "🚀 Запуск миграций для всех модулей..."

# Массив со списком скриптов
scripts=(
  "migrate_accounts_module.sh"
  "migrate_discussions_module.sh"
  "migrate_species_module.sh"
  "migrate_volunteers_module.sh"
  "migrate_volunteers_request_module.sh"
)

# Последовательный вызов каждого скрипта
for script in "${scripts[@]}"; do
  if [[ -x "$script" ]]; then
    echo "▶ Выполнение $script..."
    ./"$script"
    if [[ $? -ne 0 ]]; then
      echo "❌ Ошибка при выполнении $script. Остановка."
      exit 1
    fi
  else
    echo "⚠️  Скрипт $script не найден или не имеет права на исполнение."
  fi
done

echo "✅ Все миграции успешно применены!"

