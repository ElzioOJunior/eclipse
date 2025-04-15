FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7777

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar o arquivo .csproj
COPY ["src/EclipseApi.Api/EclipseApi.Api.csproj", "EclipseApi.Api/"]
COPY ["src/EclipseApi.Domain/EclipseApi.Domain.csproj", "src/EclipseApi.Domain/"]
COPY ["src/EclipseApi.Application/EclipseApi.Application.csproj", "EclipseApi.Application/"]
COPY ["src/EclipseApi.Infrastructure/EclipseApi.Infrastructure.csproj", "EclipseApi.Infrastructure/"]

# Restaurar dependências do projeto
RUN dotnet restore "EclipseApi.Api/EclipseApi.Api.csproj"


# Copiar os arquivos restantes
COPY /src .


# Compilar o projeto
WORKDIR "/src/EclipseApi.Api"
RUN dotnet build "EclipseApi.Api.csproj" -c Release -o /app/build

# Publicar os binários
FROM build AS publish
RUN dotnet publish "EclipseApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração final para execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "EclipseApi.dll"]
