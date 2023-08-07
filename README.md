-----------------------------------------Creation----------------------------------------------------
dotnet new webapi --name DotnetAPI  
-----------------------------------------Dapper---------------------------------------------
dotnet add package dapper  
dotnet add package Microsoft.Data.SqlClient
-----------------------------------------EntityFrameWork---------------------------------------------
1. Install EntityFrameWork: dotnet add package Microsoft.EntityFrameworkCore
2. If is a relational db :dotnet add package Microsoft.EntityFrameworkCore.Relational
3. For SQL Server: dotnet add package Microsoft.EntityFrameworkCore.SqlServer

----------------------------------------- JWT Validation -----------------------
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
