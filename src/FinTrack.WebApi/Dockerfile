# syntax=docker/dockerfile:1

# --- Etapa 1: build ---
# Usamos la imagen del SDK (pesada, ~800MB) solo para compilar.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos primero solo los .csproj para aprovechar el cache de capas de Docker:
# si el código cambia pero no las dependencias, "dotnet restore" no se repite.
COPY ["src/FinTrack.Domain/FinTrack.Domain.csproj", "src/FinTrack.Domain/"]
COPY ["src/FinTrack.Application/FinTrack.Application.csproj", "src/FinTrack.Application/"]
COPY ["src/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj", "src/FinTrack.Infrastructure/"]
COPY ["src/FinTrack.WebApi/FinTrack.WebApi.csproj", "src/FinTrack.WebApi/"]

RUN dotnet restore "src/FinTrack.WebApi/FinTrack.WebApi.csproj"

# Ahora sí copiamos todo el código fuente y compilamos.
COPY . .
WORKDIR "/src/src/FinTrack.WebApi"
RUN dotnet build "FinTrack.WebApi.csproj" -c Release -o /app/build

# --- Etapa 2: publish ---
FROM build AS publish
RUN dotnet publish "FinTrack.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa 3: runtime ---
# Imagen final: solo el runtime de ASP.NET (mucho más liviana, ~110MB),
# sin el SDK completo. Esto es lo que realmente se sube a ACR/Docker Hub.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "FinTrack.WebApi.dll"]
