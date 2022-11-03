FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY SmartPartyApi/*.csproj ./
RUN dotnet restore

COPY SmartPartyApi ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SmartPartyApi.dll"]
