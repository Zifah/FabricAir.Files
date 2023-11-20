# FabricAir.Files
## Summary
1. Implemented all **"Expected functionality"**
2. Added JWT authentication and role-based authorization
3. Authorization rules:
    - An administrator can:
        - Create a new user
        - Get user info by email address
        - Get all users
        - Get a list of files accessible to any user
        - Get a list of file groups accessible to any user
    - Any user can:
        - Get a list of files accessible to them
        - Get a list of file groups accessible to them
4. Substituted the term `UserGroup` with `Role` in my implementation

## Running locally
To run the API on your machine, you will need to run migrations to initialize your local SQLite database:
1. Open `Package Manager Console` in Visual Studio.
2. Set the `Default Project` to `FabricAir.Files.Data`.
3. Navigate to the project directory: `FabricAir.Files.Data` in the console.
4. Run the command: `dotnet ef --startup-project ..\FabricAir.Files.Api database update`. This will create your local database

The first time you run the application in the development environment, some seed data will be loaded into the database, so you can test functionality right away.