# Maintenance Guide for WeCodeCoffee

## Prerequisites
- Visual Studio 2019 or later
- .NET Core SDK
- SQL Server
- Git

## Setting Up the Application

### 1. Clone the Repository

git clone https://gitlab.com/wgu-gitlab-environment/student-repos/msm1418/d424-software-engineering-capstone.git
cd wecodecoffee

### 2. Update Connection String
Create a new database in SQL Server.
Update the connection string in appsettings.json with your database details:
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=wecodecoffee;User Id=your_user;Password=your_password;"
}

### 3. Migrations
When deploying new features that require database changes, ensure migrations are created and applied correctly.
Use the following commands in the Package Manager Console.
Add-Migration MigrationName
Update-Database

### 4. Seed Data
Run the following SQL script to seed the database with initial data:
dotnet run seeddata

### 5. Update Dependencies
Run the following command to update the NuGet packages:
dotnet restore

