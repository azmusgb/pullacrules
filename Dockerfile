# Use a specific version of the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the `src` directory and its content
COPY src/ src/
RUN dotnet publish src/NewPullACRules.csproj -c Release -o /app/build

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/build .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "NewPullACRules.dll"]
