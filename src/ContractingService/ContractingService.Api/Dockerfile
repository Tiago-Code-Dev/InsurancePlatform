# Imagem base para rodar o serviço
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Imagem para build (compilação e publish)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto primeiro (melhora cache de build)
COPY src/ContractingService/ContractingService.Api/ContractingService.Api.csproj src/ContractingService/ContractingService.Api/
COPY src/ContractingService/ContractingService.Application/ContractingService.Application.csproj src/ContractingService/ContractingService.Application/
COPY src/ContractingService/ContractingService.Domain/ContractingService.Domain.csproj src/ContractingService/ContractingService.Domain/
COPY src/ContractingService/ContractingService.Infrastructure/ContractingService.Infrastructure.csproj src/ContractingService/ContractingService.Infrastructure/
COPY src/ProposalService/ProposalService.Application/ProposalService.Application.csproj src/ProposalService/ProposalService.Application/
COPY src/ProposalService/ProposalService.Domain/ProposalService.Domain.csproj src/ProposalService/ProposalService.Domain/
COPY src/Shared/Shared.Contracts/Shared.Contracts.csproj src/Shared/Shared.Contracts/
COPY src/Shared/Shared.CrossCutting/Shared.CrossCutting.csproj src/Shared/Shared.CrossCutting/

# Restaura dependências
RUN dotnet restore src/ContractingService/ContractingService.Api/ContractingService.Api.csproj

# Copia todo o código da solução
COPY . .

# Build da API
WORKDIR /src/src/ContractingService/ContractingService.Api
RUN dotnet build ContractingService.Api.csproj -c Release -o /app/build

# Publish da API (gera pasta enxuta com binários prontos para rodar)
FROM build AS publish
RUN dotnet publish ContractingService.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Imagem final só com runtime + publicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContractingService.Api.dll"]
