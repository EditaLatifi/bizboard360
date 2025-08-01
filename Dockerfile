# 1. Use .NET runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# 2. Use SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Finlab.csproj", "./"]
RUN dotnet restore

# 3. Copy everything and publish it
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 4. Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Finlab.dll"]
