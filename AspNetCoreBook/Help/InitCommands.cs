// Needed packages:
// Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
// Install-Package Microsoft.EntityFrameworkCore.Tools
// Install-Package Microsoft.EntityFrameworkCore.Design
// Install-Package Swashbuckle.AspNetCore
// Install-Package Microsoft.AspNetCore.OpenApi -Version 8.0.8

// Needed Postgre SQL:
// From https://www.enterprisedb.com/download-postgresql-binaries (postgresql-17.6-1-windows-x64-binaries.zip)
// c:\PostgreSQL-17\bin\initdb.exe -D C:\PostgreSQL-17\data -U postgres -A md5 -E UTF8 -W
// user: postgres, password: 12345678
// c:\PostgreSQL-17\bin\pg_ctl.exe -D C:\PostgreSQL-17\data start
// c:\PostgreSQL-17\bin\pg_ctl.exe -D C:\PostgreSQL-17\data status
// c:\PostgreSQL-17\bin\pg_ctl.exe -D C:\PostgreSQL-17\data restart
// c:\PostgreSQL-17\bin\pg_ctl.exe -D C:\PostgreSQL-17\data stop
// c:\PostgreSQL-17\bin\psql.exe -U postgres -h localhost -p 5432 -c "CREATE DATABASE bookdb;"
// c:\PostgreSQL-17\bin\psql.exe -U postgres -h localhost -p 5432 -d bookdb
// bookdb=# \q

// Needed instrument: dotnet-ef
// dotnet tool install -g dotnet-ef
// dotnet tool list -g
// echo %USERPROFILE%
// dir "%USERPROFILE%\.dotnet\tools"
// Need add Windows global Path:
// Win+R: sysdm.cpl
// "Свойства системы"->"Дополнительно"->"Переменные среды"->"Path"->"Изменить"->"Создать"->"%USERPROFILE%\.dotnet\tools"->"ОК"->"ОК"->"ОК
// Latest settings:
// dotnet ef --version // where dotnet-ef
// cd C:\Users\boral\source\repos\AspNetCoreBook
// dotnet ef migrations add InitialCreate --project AspNetCoreBook --startup-project AspNetCoreBook
// cd C:\Users\boral\source\repos\AspNetCoreBook\AspNetCoreBook
// dotnet ef database update

// Check DB:
// C:\PostgreSQL-17\bin\psql.exe -U postgres -h localhost -p 5432 -d bookdb
// bookdb=# \dt
// bookdb=# \d "Books"
// bookdb=# SELECT * FROM "Books";

// In Debian:
// sudo apt update
// sudo apt upgrade
// sudo apt install -y apt-transport-https ca-certificates curl gnupg lsb-release
// curl -fsSL https://download.docker.com/linux/debian/gpg | sudo gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg
// echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
// sudo apt install -y docker-ce docker-ce-cli containerd.io
// sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
// sudo chmod +x /usr/local/bin/docker-compose
// docker --version
// docker-compose --version
// cd /mnt/c/Users/boral/source/repos/AspNetCoreBook
// docker-compose up --build
// docker-compose up --build -d // run in background
// docker ps -a
// docker-compose down

// Unit-тесты с xUnit и EF Core In-Memory
// cd C:\Users\boral\source\repos\AspNetCoreBook
// dotnet new xunit -n AspNetCoreBook.Tests
// dotnet add AspNetCoreBook.Tests reference AspNetCoreBook/AspNetCoreBook.csproj
// cd AspNetCoreBook.Tests
// dotnet add package Microsoft.EntityFrameworkCore.InMemory
// cd C:\Users\boral\source\repos\AspNetCoreBook
// dotnet test
