# Build container. Load full SDK as base image.TaskTrackerProject
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# TODO: Adapt the directories!
COPY TaskTrackerProject.Application TaskTrackerProject.Application
COPY TaskTrackerProject.Webapi      TaskTrackerProject.Webapi

# Compile the app
RUN dotnet restore "TaskTrackerProject.Webapi"
RUN dotnet build   "TaskTrackerProject.Webapi" -c Release -o /app/build
RUN dotnet publish "TaskTrackerProject.Webapi" -c Release -o /app/publish /p:UseAppHost=false

# App container. Only needs runtime (smaller image)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
EXPOSE 80
EXPOSE 443
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskTrackerProject.Webapi.dll"]