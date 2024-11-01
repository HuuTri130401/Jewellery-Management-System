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
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./src/THT.JMS.API/THT.JMS.API.csproj", "./src/THT.JMS.API/"]
COPY ["./src/THT.JMS.Application/THT.JMS.Application.csproj", "./src/THT.JMS.Application/"]
COPY ["./src/THT.JMS.Domain/THT.JMS.Domain.csproj", "./src/THT.JMS.Domain/"]
COPY ["./src/THT.JMS.Persistence/THT.JMS.Persistence.csproj", "./src/THT.JMS.Persistence/"]
COPY ["./src/THT.JMS.Utilities/THT.JMS.Utilities.csproj", "./src/THT.JMS.Utilities/"]

# run restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore "./src/THT.JMS.API/THT.JMS.API.csproj"

COPY . .

# run build over the API project
WORKDIR "./src/THT.JMS.API"
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT ["dotnet", "THT.JMS.API.dll"]


