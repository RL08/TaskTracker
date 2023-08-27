docker rm -f TaskTracker_sqlserver &> nul
docker run -d -p 11433:1433 --name TaskTracker_sqlserver -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer2019" mcr.microsoft.com/azure-sql-edge
dotnet build TaskTrackerProject\TaskTrackerProject.Webapi --no-incremental --force
dotnet watch run -c Debug --project TaskTrackerProject\TaskTrackerProject.Webapi