CREATE DATABASE CompanyStructureDB;
GO
USE CompanyStructureDB;

CREATE TABLE Employees (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);

CREATE TABLE Companies (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(100) NOT NULL,
    CeoId INT,
    FOREIGN KEY (CeoId) REFERENCES Employees(Id)
);

CREATE TABLE Divisions (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(100) NOT NULL,
    CompanyId INT,
    DirectorId INT,
    FOREIGN KEY (CompanyId) REFERENCES Companies(Id),
    FOREIGN KEY (DirectorId) REFERENCES Employees(Id)
);

CREATE TABLE Projects (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(100) NOT NULL,
    DivisionId INT,
    ManagerId INT,
    FOREIGN KEY (DivisionId) REFERENCES Divisions(Id),
    FOREIGN KEY (ManagerId) REFERENCES Employees(Id)
);

CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    Code NVARCHAR(100) NOT NULL,
    ProjectId INT,
    ManagerId INT,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id),
    FOREIGN KEY (ManagerId) REFERENCES Employees(Id)
);