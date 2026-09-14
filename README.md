# NationalCodeCostProject

An ASP.NET Core MVC application for managing cost information based on Iranian national codes.

## Features

* Import cost data from Excel files
* Validate imported data
* Search by national code
* Filter and paginate cost records
* Admin authentication and authorization
* View basic cost reports

## Tech Stack

* C#
* .NET 8
* ASP.NET Core MVC
* Entity Framework Core
* SQLite
* ClosedXML
* Razor
* Git & GitHub

## Project Structure

```text
Controllers
Data
Dto
Models
Services
Views
```

The project uses a simple layered structure with Controllers, Services, and Data Access separated.

## Getting Started

```bash
git clone https://github.com/MahdiEbrahimiDev/NationalCodeCostProject.git
cd NationalCodeCostProject
dotnet restore
dotnet run
```

## Author

**Mahdi Ebrahimi**

GitHub: [MahdiEbrahimiDev](https://github.com/MahdiEbrahimiDev)
