# Imagem base: runtime do .NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem de build: SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto (para cache eficiente)
COPY src/ProposalService/ProposalService.Api/ProposalService.Api.csproj src/ProposalService/ProposalService.Api/
COPY src/ProposalService/ProposalService.Application/ProposalService.Application.csproj src/ProposalService/ProposalService.Application/
COPY src/ProposalService/ProposalService.Domain/ProposalService.Domain.csproj src/ProposalService/ProposalService.Domain/
COPY src/ProposalService/ProposalService.Infrastructure/ProposalService.Infrastructure.csproj src/ProposalService/ProposalService.Infrastructure/
COPY src/Shared/Shared.Contracts/Shared.Contracts.csproj src/Shared/Shared.Contracts/
COPY src/Shared/Shared.CrossCutting/Shared.CrossCutting.csproj src/Shared/Shared.CrossCutting/

# Restaura pacotes
RUN dotnet restore src/ProposalService/ProposalService.Api/ProposalService.Api.csproj

# Copia todo o código
COPY . .

# Build da API
WORKDIR /src/src/ProposalService/ProposalService.Api
RUN dotnet build ProposalService.Api.csproj -c Release -o /app/build

# Publish (gera binários enxutos)
FROM build AS publish
RUN dotnet publish ProposalService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Imagem final com runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProposalService.Api.dll"]
