# ��. https://aka.ms/customizecontainer ��� ��������� �������
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8077
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BankAPI/BankAPI.csproj", "BankAPI/"]
RUN dotnet restore "./BankAPI/BankAPI.csproj"
COPY . .
WORKDIR "/src/BankAPI"
RUN dotnet build "./BankAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BankAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ��������� curl � ��������� �����
USER root
RUN apt-get update && apt-get install -y curl
USER app

ENTRYPOINT ["dotnet", "BankAPI.dll"]