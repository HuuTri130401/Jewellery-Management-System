# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 4343
# #ENV ASPNETCORE_URLS="http://+:8080;https://+:4343"

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# # COPY *.csproj ./
# COPY ["./THT.JMS.API/THT.JMS.API.csproj", "src/THT.JMS.API/"]
# COPY ["./THT.JMS.Application/THT.JMS.Application.csproj", "src/THT.JMS.Application/"]
# COPY ["./THT.JMS.Domain/THT.JMS.Domain.csproj", "src/THT.JMS.Domain/"]
# COPY ["./THT.JMS.Persistence/THT.JMS.Persistence.csproj", "src/THT.JMS.Persistence/"]
# COPY ["./THT.JMS.Utilities/THT.JMS.Utilities.csproj", "src/THT.JMS.Utilities/"]

# RUN dotnet restore "src/THT.JMS.API/THT.JMS.API.csproj"

# COPY . .

# WORKDIR "/src/THT.JMS.API"
# RUN dotnet build "THT.JMS.API.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "THT.JMS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "THT.JMS.API.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["./THT.JMS.API/THT.JMS.API.csproj", "src/THT.JMS.API/"]
COPY ["./THT.JMS.Application/THT.JMS.Application.csproj", "src/THT.JMS.Application/"]
COPY ["./THT.JMS.Domain/THT.JMS.Domain.csproj", "src/THT.JMS.Domain/"]
COPY ["./THT.JMS.Persistence/THT.JMS.Persistence.csproj", "src/THT.JMS.Persistence/"]
COPY ["./THT.JMS.Utilities/THT.JMS.Utilities.csproj", "src/THT.JMS.Utilities/"]

RUN dotnet restore "src/THT.JMS.API/THT.JMS.API.csproj"

COPY . .
WORKDIR "/src/THT.JMS.API"
RUN dotnet build "THT.JMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "THT.JMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "THT.JMS.API.dll","--server.urls","http://*/5000"]