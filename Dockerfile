FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copy the .csproj and restore any dependencies
COPY ["FoodApi.csproj", "./"]
RUN dotnet restore "./FoodApi.csproj"

# Copy the remaining source code and build the app
COPY . .
WORKDIR "/src/"
RUN dotnet build "FoodApi.csproj" -c Release -o /app/build

# Publish the app to a folder
FROM build AS publish
RUN dotnet publish "FoodApi.csproj" -c Release -o /app/publish

# Use the base image and copy the published app to the app directory
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

# Set the entry point to run the app
ENTRYPOINT ["dotnet", "FoodApi.dll"]