# Festify

An example [Immutable Architecture](https://immutablearchitecture.com).

## Prerequisites

Please install:

* Dot Net Core
* SQL Server Developer Edition

## Getting Started

Clone the repository.
Build with:

```powershell
dotnet build
```

Run the tests with:

```powershell
dotnet test
```

Before you run, create your database.
The database will be created in your local SQL Server instance and be named `festify`.

```powershell
dotnet ef database update --project .\Festify.Promotion\
```

Then to run the application:

```powershell
dotnet run --project .\Festify.Promotion\
```

Then you can hit the API at: [https://localhost:5001/shows](https://localhost:5001/shows).