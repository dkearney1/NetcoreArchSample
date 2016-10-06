FROM microsoft/dotnet:1.0.1-core

MAINTAINER dkearney1@gmail.com

LABEL	version = "1.0" \
	description = "Contact Service"

COPY ./appsettings.json /app/
COPY ./bin/release/netcoreapp1.0/publish /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "ContactService.dll"]
