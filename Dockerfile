# 1. stage: build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY EquipmentTrackerThesis/EquipmentTrackerThesis.csproj EquipmentTrackerThesis/
RUN dotnet restore EquipmentTrackerThesis/EquipmentTrackerThesis.csproj

COPY . .
WORKDIR /src/EquipmentTrackerThesis
RUN dotnet publish -c Release -o /app/publish

# 2. stage: runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Adding SQL file to the app directory 
COPY EquipmentTrackerThesis/ManagementSystemDB.sql .
ENTRYPOINT ["dotnet", "EquipmentTrackerThesis.dll"]