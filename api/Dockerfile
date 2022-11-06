FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY SmartPartyApi/*.csproj ./
RUN dotnet restore

COPY SmartPartyApi ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

ARG DB_URL="mongodb:27017"
ARG DB_NAME="SmartParty"

RUN sed -i "s/db_url/$DB_URL/" ./database.json
RUN sed -i "s/db_name/$DB_NAME/" ./database.json

ENTRYPOINT ["dotnet", "SmartPartyApi.dll"]
