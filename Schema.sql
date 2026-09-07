-- 23-55642-3_CompanyApp
-- Unified database schema

CREATE DATABASE dbCompanyApp;
GO

USE dbCompanyApp;
GO

CREATE TABLE dbo.Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE dbo.Emp_details
(
    EmpId NVARCHAR(50) PRIMARY KEY,
    EmpName NVARCHAR(100) NOT NULL,
    EmpAge INT NOT NULL,
    EmpContact NVARCHAR(20),
    EmpGender NVARCHAR(10),
    CreatedBy INT NULL,
    CONSTRAINT FK_Emp_CreatedBy
        FOREIGN KEY (CreatedBy)
        REFERENCES dbo.Users(UserID)
);
GO

-- Access migration example:
-- Do not supply UserID; SQL Server generates it.
-- INSERT INTO dbo.Users (Username, Password)
-- VALUES ('your_username', 'your_password');
