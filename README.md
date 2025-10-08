# Simple REST API with C# and PostgreSQL

A simple REST API built with ASP.NET Core 8.0 and PostgreSQL database for managing tasks.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- Visual Studio Code or Visual Studio 2022

## Required Packages

This project uses the following NuGet packages:

```bash
# Entity Framework Core for PostgreSQL
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Entity Framework Core Design (for migrations)
dotnet add package Microsoft.EntityFrameworkCore.Design
```

## Database Configuration

1. Make sure PostgreSQL is installed and running
2. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=simple_rest_c;User Id=yourusername;Password=yourpassword;"
  }
}
```

## Setup and Running

1. Clone the repository
```bash
git clone https://github.com/bayuramadhans/simple-rest-c-sharp.git
cd simple-rest-c-sharp
```

2. Install required packages
```bash
dotnet restore
```

3. Create and apply database migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application
```bash
dotnet run
```

The API will be available at:
- API Endpoints: `https://localhost:5001` or `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks/{id}` | Get a specific task |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update an existing task |
| DELETE | `/api/tasks/{id}` | Delete a task |

### Request/Response Examples

#### Create a Task
```json
POST /api/tasks
{
    "title": "Learn C#",
    "isCompleted": false
}
```

#### Get All Tasks Response
```json
[
    {
        "id": 1,
        "title": "Learn C#",
        "isCompleted": false
    }
]
```

## Project Structure

```
SimpleRestApi/
├── Program.cs              # Main application entry point and API endpoints
├── appsettings.json       # Application configuration
├── SimpleRestApi.csproj   # Project file
└── .gitignore            # Git ignore file
```

## Features

- RESTful API endpoints for CRUD operations
- PostgreSQL database integration
- Automatic database migrations
- Swagger UI for API documentation
- Error handling and logging
- Async/await implementation

## Development

The application uses:
- Minimal API approach
- Entity Framework Core for database operations
- Swagger for API documentation
- PostgreSQL as the database

## Error Handling

The application includes basic error handling for:
- Database connection issues
- Invalid requests
- Not found resources
- Server errors

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is open source and available under the [MIT License](LICENSE).