# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish /src/NewPullACRules.csproj -c Release -o /app/build

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "NewPullACRules.dll"]
