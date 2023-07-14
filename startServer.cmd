@echo off
docker rm -f TaskTracker_sqlserver 2> nul
docker run -d -p 11433:1433 --name TaskTracker_sqlserver -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer2019" mcr.microsoft.com/azure-sql-edge
docker run -d -p 5000:80 --name tasktracker_app tasktracker_app
IF ERRORLEVEL 1 GOTO error
dotnet build TaskTrackerProject\TaskTrackerProject.Webapi --no-incremental --force
dotnet watch run -c Debug --project TaskTrackerProject\TaskTrackerProject.Webapi
GOTO end

:error
PAUSE
:end