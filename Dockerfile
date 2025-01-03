# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .  # Copies all files to the /app directory inside the container
RUN dotnet publish ./src/NewPullACRules.csproj -c Release -o /app/build

# Use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "NewPullACRules.dll"]
