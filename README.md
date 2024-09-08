# Company Structure API

This API allows managing a company structure stored in a SQL Server database. The API provides CRUD operations for employees, companies, divisions, projects, and departments.

## Prerequisites

- **SQL Server** installed and running
- **SQL Server Management Studio (SSMS)** or any SQL client to run the SQL script
- **Postman** (optional, but recommended for testing the API)

## Setup

1. **Database Setup**:
   - The SQL script for creating the database and tables is located at:  
     `.\CreatingDatabaseAndTables.sql`.
   - Execute this script to create a database named `CompanyStructureDB` with the following tables:
     - `Employees`
     - `Companies`
     - `Divisions`
     - `Projects`
     - `Departments`
   
2. **Connection String**:
   - You need to set up the connection to your database in the `launchSettings.json` file:
     - Path: `.\CompanyStructAPI\Properties\launchSettings.json`
     - Modify the **DefaultConnection** string to match your SQL Server instance and database location. Example:
       ```json
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CompanyStructureDB;Trusted_Connection=True;TrustServerCertificate=True;"
       ```

## Running the API

After setting up the database and configuring the connection string, you can run the API locally or deploy it to your preferred environment.

- By default, the API runs over HTTPS on port `7080`.
- You can use **Postman** to send requests to the API endpoints. For example:
  - **Base URL**: `https://localhost:7080/`
  - Use Postman to test different API routes and verify the functionality of CRUD operations.

### API Endpoints

Each table in the database has corresponding API endpoints:

- **Employees**:  
  - `GET`, `POST`, `PUT`, and `DELETE` requests can be sent to `https://localhost:7080/employees`.
  - For example, to get details of an employee with ID 1, use:  
    `https://localhost:7080/employees/1`.

- **Companies**:  
  - API requests for companies can be sent to `https://localhost:7080/companies`.
  - For example, to access company with ID 1:  
    `https://localhost:7080/companies/1`.

- **Divisions**:  
  - API requests for divisions are at `https://localhost:7080/divisions`.
  - Access specific division with ID 1:  
    `https://localhost:7080/divisions/1`.

- **Projects**:  
  - Use `https://localhost:7080/projects` for project-related requests.
  - Access specific project by ID:  
    `https://localhost:7080/projects/1`.

- **Departments**:  
  - Requests for departments go to `https://localhost:7080/departments`.
  - To access department with ID 1:  
    `https://localhost:7080/departments/1`.

Each entity can be managed via standard HTTP methods (GET, POST, PUT, DELETE), and the specific entity is accessed via its unique ID in the URL.
