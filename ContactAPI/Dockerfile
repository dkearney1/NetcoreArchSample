FROM microsoft/dotnet:1.0.1-core

MAINTAINER dkearney1@gmail.com

LABEL	version = "1.0" \
	description = "Contact Web API"

COPY ./appsettings.json /app/
COPY ./bin/release/netcoreapp1.0/publish /app/

WORKDIR /app

EXPOSE 5000/tcp

ENTRYPOINT ["dotnet", "ContactAPI.dll"]
