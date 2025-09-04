# Base
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /usr/src

ARG CONFIGURATION="Release"

RUN chown root:root -R /usr/src

# Copy common files
COPY ./nuget.config ./*.props ./src/*.sln ./

# Copy library files
COPY src/*/*.csproj ./src/
RUN for file in $(ls ./src/*.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

# restore .NET dependencies
RUN --mount=type=secret,id=nuget,dst=/usr/nuget.config \
    dotnet restore "src/TelegramBotPrototype";

# copy remaining files
COPY . .

# build
RUN dotnet build "src/TelegramBotPrototype" -c $CONFIGURATION --no-restore

# publish
RUN dotnet publish "src/TelegramBotPrototype" -c $CONFIGURATION -o /srv --no-build --no-restore

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
COPY --from=base /srv /srv
ENTRYPOINT [ "dotnet", "srv/TelegramBotPrototype.dll" ]
