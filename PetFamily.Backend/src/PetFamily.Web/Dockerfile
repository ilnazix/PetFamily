#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.Web/PetFamily.Web.csproj", "src/PetFamily.Web/"]
COPY ["src/Accounts/PetFamily.Accounts.Presentation/PetFamily.Accounts.Presentation.csproj", "src/Accounts/PetFamily.Accounts.Presentation/"]
COPY ["src/Shared/PetFamily.Framework/PetFamily.Framework.csproj", "src/Shared/PetFamily.Framework/"]
COPY ["src/Accounts/PetFamily.Accounts.Contracts/PetFamily.Accounts.Contracts.csproj", "src/Accounts/PetFamily.Accounts.Contracts/"]
COPY ["src/Shared/PetFamily.Core/PetFamily.Core.csproj", "src/Shared/PetFamily.Core/"]
COPY ["src/Shared/PetFamily.SharedKernel/PetFamily.SharedKernel.csproj", "src/Shared/PetFamily.SharedKernel/"]
COPY ["src/Accounts/PetFamily.Accounts.Application/PetFamily.Accounts.Application.csproj", "src/Accounts/PetFamily.Accounts.Application/"]
COPY ["src/Accounts/PetFamily.Accounts.Domain/PetFamily.Accounts.Domain.csproj", "src/Accounts/PetFamily.Accounts.Domain/"]
COPY ["src/Accounts/PetFamily.Accounts.Infrastructure/PetFamily.Accounts.Infrastructure.csproj", "src/Accounts/PetFamily.Accounts.Infrastructure/"]
COPY ["src/Discussions/PetFamily.Discussions.Presentation/PetFamily.Discussions.Presentation.csproj", "src/Discussions/PetFamily.Discussions.Presentation/"]
COPY ["src/Discussions/PetFamily.Discussions.Application/PetFamily.Discussions.Application.csproj", "src/Discussions/PetFamily.Discussions.Application/"]
COPY ["src/Discussions/PetFamily.Discussions.Domain/PetFamily.Discussions.Domain.csproj", "src/Discussions/PetFamily.Discussions.Domain/"]
COPY ["src/Discussions/PetFamily.Discussions.Contracts/PetFamily.Discussions.Contracts.csproj", "src/Discussions/PetFamily.Discussions.Contracts/"]
COPY ["src/Discussions/PetFamily.Discussions.Infrastructure/PetFamily.Discussions.Infrastructure.csproj", "src/Discussions/PetFamily.Discussions.Infrastructure/"]
COPY ["src/Species/PetFamily.Species.Presentation/PetFamily.Species.Presentation.csproj", "src/Species/PetFamily.Species.Presentation/"]
COPY ["src/Species/PetFamily.Species.Application/PetFamily.Species.Application.csproj", "src/Species/PetFamily.Species.Application/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Contracts/PetFamily.Volunteers.Contracts.csproj", "src/Volunteers/PetFamily.Volunteers.Contracts/"]
COPY ["src/Species/PetFamily.Species.Domain/PetFamily.Species.Domain.csproj", "src/Species/PetFamily.Species.Domain/"]
COPY ["src/Species/PetFamily.Species.Contracts/PetFamily.Species.Contracts.csproj", "src/Species/PetFamily.Species.Contracts/"]
COPY ["src/Species/PetFamily.Species.Infrastructure/PetFamily.Species.Infrastructure.csproj", "src/Species/PetFamily.Species.Infrastructure/"]
COPY ["src/VolunteerRequest/PetFamily.VolunteerRequest.Presentation/PetFamily.VolunteerRequest.Presentation.csproj", "src/VolunteerRequest/PetFamily.VolunteerRequest.Presentation/"]
COPY ["src/VolunteerRequest/PetFamily.VolunteerRequest.Application/PetFamily.VolunteerRequest.Application.csproj", "src/VolunteerRequest/PetFamily.VolunteerRequest.Application/"]
COPY ["src/VolunteerRequest/PetFamily.VolunteerRequest.Domain/PetFamily.VolunteerRequest.Domain.csproj", "src/VolunteerRequest/PetFamily.VolunteerRequest.Domain/"]
COPY ["src/VolunteerRequest/PetFamily.VolunteerRequest.Contracts/PetFamily.VolunteerRequest.Contracts.csproj", "src/VolunteerRequest/PetFamily.VolunteerRequest.Contracts/"]
COPY ["src/VolunteerRequest/PetFamily.VolunteerRequest.Infrastructure/PetFamily.VolunteerRequest.Infrastructure.csproj", "src/VolunteerRequest/PetFamily.VolunteerRequest.Infrastructure/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Presentation/PetFamily.Volunteers.Presentation.csproj", "src/Volunteers/PetFamily.Volunteers.Presentation/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Application/PetFamily.Volunteers.Application.csproj", "src/Volunteers/PetFamily.Volunteers.Application/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Domain/PetFamily.Volunteers.Domain.csproj", "src/Volunteers/PetFamily.Volunteers.Domain/"]
COPY ["src/Volunteers/PetFamily.Volunteers.Infrastructure/PetFamily.Volunteers.Infrastructure.csproj", "src/Volunteers/PetFamily.Volunteers.Infrastructure/"]
RUN dotnet restore "./src/PetFamily.Web/PetFamily.Web.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.Web"
RUN dotnet build "./PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.Web.dll"]