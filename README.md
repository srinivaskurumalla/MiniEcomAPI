# MiniEcom.Api

Generated minimal .NET 8 Web API for Mini E-commerce demo.

## Setup

1. Ensure .NET 8 SDK is installed.
2. Ensure SQL Server is available and run the SQL schema (the schema SQL was provided earlier).
3. Update connection string in appsettings.json if necessary.
4. From the project folder run:
   ```bash
   dotnet restore
   dotnet ef database update
   dotnet run
   ```
5. Swagger UI available in development at `/swagger`.

Notes:
- JWT configuration is present; implement login/token endpoints to issue tokens.
- Order creation uses stored procedure `usp_CreateOrderAndDeductStock` included in the DB schema.
