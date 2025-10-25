#!/bin/bash

CONTEXT="AccountsWriteDbContext"
STARTUP_PROJECT="PetFamily.Web/"
PROJECT="Accounts/PetFamily.Accounts.Infrastructure"
TIMESTAMP=$(date +"%Y%m%d%H%M%S")

dotnet ef migrations add "${CONTEXT}_${TIMESTAMP}" \
    --context "$CONTEXT" \
    --startup-project "$STARTUP_PROJECT" \
    --project "$PROJECT"

dotnet ef database update \
    --context "$CONTEXT" \
    --startup-project "$STARTUP_PROJECT" \
    --project "$PROJECT"


