FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetFamily.Web/PetFamily.Web.csproj", "src/PetFamily.Web/"]
COPY ["src/Accounts/src/PetFamily.Accounts.Application/PetFamily.Accounts.Application.csproj", "src/Accounts/src/PetFamily.Accounts.Application/"]
COPY ["src/Shared/PetFamily.Core/PetFamily.Core.csproj", "src/Shared/PetFamily.Core/"]
COPY ["src/Shared/PetFamily.SharedKernel/PetFamily.SharedKernel.csproj", "src/Shared/PetFamily.SharedKernel/"]
COPY ["src/Shared/PetFamily.Framework/PetFamily.Framework.csproj", "src/Shared/PetFamily.Framework/"]
COPY ["src/Accounts/src/PetFamily.Accounts.Contracts/PetFamily.Accounts.Contracts.csproj", "src/Accounts/src/PetFamily.Accounts.Contracts/"]
COPY ["src/Volunteers/src/PetFamily.Volunteers.Contracts/PetFamily.Volunteers.Contracts.csproj", "src/Volunteers/src/PetFamily.Volunteers.Contracts/"]
COPY ["src/Accounts/src/PetFamily.Accounts.Domain/PetFamily.Accounts.Domain.csproj", "src/Accounts/src/PetFamily.Accounts.Domain/"]
COPY ["src/Accounts/src/PetFamily.Accounts.Infrastructure/PetFamily.Accounts.Infrastructure.csproj", "src/Accounts/src/PetFamily.Accounts.Infrastructure/"]
COPY ["src/Accounts/src/PetFamily.Accounts.Presentation/PetFamily.Accounts.Presentation.csproj", "src/Accounts/src/PetFamily.Accounts.Presentation/"]
COPY ["src/Species/src/PetFamily.Species.Application/PetFamily.Species.Application.csproj", "src/Species/src/PetFamily.Species.Application/"]
COPY ["src/Species/src/PetFamily.Species.Contracts/PetFamily.Species.Contracts.csproj", "src/Species/src/PetFamily.Species.Contracts/"]
COPY ["src/Species/src/PetFamily.Species.Domain/PetFamily.Species.Domain.csproj", "src/Species/src/PetFamily.Species.Domain/"]
COPY ["src/Species/src/PetFamily.Species.Infrastructure/PetFamily.Species.Infrastructure.csproj", "src/Species/src/PetFamily.Species.Infrastructure/"]
COPY ["src/Species/src/PetFamily.Species.Presentation/PetFamily.Species.Presentation.csproj", "src/Species/src/PetFamily.Species.Presentation/"]
COPY ["src/Volunteers/src/PetFamily.Volunteers.Application/PetFamily.Volunteers.Application.csproj", "src/Volunteers/src/PetFamily.Volunteers.Application/"]
COPY ["src/Volunteers/src/PetFamily.Volunteers.Domain/PetFamily.Volunteers.Domain.csproj", "src/Volunteers/src/PetFamily.Volunteers.Domain/"]
COPY ["src/Volunteers/src/PetFamily.Volunteers.Infrastructure/PetFamily.Volunteers.Infrastructure.csproj", "src/Volunteers/src/PetFamily.Volunteers.Infrastructure/"]
COPY ["src/Volunteers/src/PetFamily.Volunteers.Presentation/PetFamily.Volunteers.Presentation.csproj", "src/Volunteers/src/PetFamily.Volunteers.Presentation/"]
COPY ["src/VolunteerRequest/src/PetFamily.VolunteerRequest.Application/PetFamily.VolunteerRequest.Application.csproj", "src/VolunteerRequest/src/PetFamily.VolunteerRequest.Application/"]
COPY ["src/Discussion/src/PetFamily.Discussion.Contracts/PetFamily.Discussion.Contracts.csproj", "src/Discussion/src/PetFamily.Discussion.Contracts/"]
COPY ["src/VolunteerRequest/src/PetFamily.VolunteerRequest.Contracts/PetFamily.VolunteerRequest.Contracts.csproj", "src/VolunteerRequest/src/PetFamily.VolunteerRequest.Contracts/"]
COPY ["src/VolunteerRequest/src/PetFamily.VolunteerRequest.Domain/PetFamily.VolunteerRequest.Domain.csproj", "src/VolunteerRequest/src/PetFamily.VolunteerRequest.Domain/"]
COPY ["src/Discussion/src/PetFamily.Discussion.Presentation/PetFamily.Discussion.Presentation.csproj", "src/Discussion/src/PetFamily.Discussion.Presentation/"]
COPY ["src/Discussion/src/PetFamily.Discussion.Application/PetFamily.Discussion.Application.csproj", "src/Discussion/src/PetFamily.Discussion.Application/"]
COPY ["src/Discussion/src/PetFamily.Discussion.Domain/PetFamily.Discussion.Domain.csproj", "src/Discussion/src/PetFamily.Discussion.Domain/"]
COPY ["src/VolunteerRequest/src/PetFamily.VolunteerRequest.Presentation/PetFamily.VolunteerRequest.Presentation.csproj", "src/VolunteerRequest/src/PetFamily.VolunteerRequest.Presentation/"]
COPY ["src/VolunteerRequest/src/PetFamily.VolunteerRequest.Infrastructure/PetFamily.VolunteerRequest.Infrastructure.csproj", "src/VolunteerRequest/src/PetFamily.VolunteerRequest.Infrastructure/"]
COPY ["src/Discussion/src/PetFamily.Discussion.Infrastructure/PetFamily.Discussion.Infrastructure.csproj", "src/Discussion/src/PetFamily.Discussion.Infrastructure/"]
RUN dotnet restore "src/PetFamily.Web/PetFamily.Web.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.Web"
RUN dotnet build "PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetFamily.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.Web.dll"]  
