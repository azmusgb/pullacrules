FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY src/ src/  # Copy the `src` directory and its content
RUN dotnet publish src/NewPullACRules.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "NewPullACRules.dll"]
