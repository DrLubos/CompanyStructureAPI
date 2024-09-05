CREATE DATABASE CompanyStructureDB;
GO
USE CompanyStructureDB;

CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(20),
    FirstName NVARCHAR(20),
    LastName NVARCHAR(20),
    Phone NVARCHAR(20),
    Email NVARCHAR(100)
);

CREATE TABLE Companies (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Code NVARCHAR(10),
    CeoId INT FOREIGN KEY REFERENCES Employees(Id)
);

CREATE TABLE Divisions (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Code NVARCHAR(10),
    CompanyId INT FOREIGN KEY REFERENCES Companies(Id),
    DivisionManagerId INT FOREIGN KEY REFERENCES Employees(Id)
);

CREATE TABLE Projects (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Code NVARCHAR(10),
    DivisionId INT FOREIGN KEY REFERENCES Divisions(Id),
    ProjectManagerId INT FOREIGN KEY REFERENCES Employees(Id)
);

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Code NVARCHAR(10),
    ProjectId INT FOREIGN KEY REFERENCES Projects(Id),
    DepartmentManagerId INT FOREIGN KEY REFERENCES Employees(Id)
);