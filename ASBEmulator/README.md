run docker containers for ASBEmulator

```shell
$env:CONFIG_PATH = "$(Get-Location)\config.json"
$env:MSSQL_SA_PASSWORD = "YourStrongPassword123!"
$env:ACCEPT_EULA = "Y"
$env:SQL_WAIT_INTERVAL = "30"

# Start the emulator and SQL Server
docker-compose up -d
```