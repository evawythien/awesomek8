FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./Awesomek8.csproj /app
RUN dotnet restore /app/Awesomek8.csproj

# Copy everything else and build
COPY . /app/

RUN dotnet publish /app/Awesomek8.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

RUN apt-get update
RUN apt-get -y --no-install-recommends install curl

COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Awesomek8.dll"]

EXPOSE 80