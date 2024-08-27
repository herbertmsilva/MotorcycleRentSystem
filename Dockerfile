FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

WORKDIR /App
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

EXPOSE 80
EXPOSE 8080

ENTRYPOINT ["dotnet", "MotorcycleRentalSystem.Api.dll"]