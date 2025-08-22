# FinLab Admin Dashboard

A clean, modern admin dashboard template built with ASP.NET Core MVC and PostgreSQL.

## Features

- **User Authentication System**
  - User registration and login
  - Secure password hashing
  - PostgreSQL database integration
- **Dashboard Features**
  - File management
  - User management
  - CMS functionality
  - E-commerce features
  - UI components and forms
  - Data tables

## Prerequisites

- .NET 8.0 SDK
- PostgreSQL database
- pgAdmin 4 (for database management)

## Setup Instructions

### 1. Database Setup

1. Install PostgreSQL on your system
2. Install pgAdmin 4 for database management
3. Create a new database named `bizboard360`
4. Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=bizboard360;Username=postgres;Password=your_actual_password"
}
```

### 2. Application Setup

1. Clone or download the project
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```
5. Run database migrations:
   ```bash
   dotnet ef database update
   ```
6. Run the application:
   ```bash
   dotnet run
   ```

### 3. Access the Application

- **Login Page**: http://localhost:5000/Account/Login
- **Register Page**: http://localhost:5000/Account/Register
- **Dashboard**: http://localhost:5000/Finlab/Index (after login)

## Default Routes

- **Account Controller**: Handles user authentication
- **Finlab Controller**: Main dashboard functionality
- **Home Controller**: Basic home pages

## Database Schema

The application uses Entity Framework Core with the following main entities:

- **Users**: User accounts with username, email, and password hash
- Additional entities can be added as needed

## Security Features

- Password hashing using SHA256
- Input validation and sanitization
- CSRF protection
- Secure database connections

## Customization

- Modify the `Views/Shared/Finlab/_Sidebar.cshtml` to customize navigation
- Update the `Views/Finlab/` folder for dashboard content
- Customize styles in `wwwroot/Finlab/css/style.css`

## Notes

- This is a dashboard template, not a crypto trading platform
- Remove unused features as needed for your specific use case
- Implement proper session management for production use
- Consider adding JWT tokens for API authentication if needed

