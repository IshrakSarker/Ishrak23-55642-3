# 25-60783-1_CompanyApp

## Lab 2: Merging Login/Register and Employee CRUD into One App

This project merges the original `Login-and-Register` WinForms solution and the original `EmployeeDetails` CRUD solution into one Windows Forms application.

### Before and after

**Before**

- Login/Register application
  - WinForms
  - `frmLogin`, `frmRegister`, `frmDashboard`
  - Microsoft Access `.mdb`
  - `System.Data.OleDb`
- Employee CRUD application
  - WinForms
  - `Form1`
  - SQL Server LocalDB
  - `System.Data.SqlClient`

**After**

- One solution: `25-60783-1_CompanyApp`
- One executable
- One SQL Server LocalDB database: `dbCompanyApp`
- Login/Register/Dashboard + Employee CRUD
- Login must succeed before Employee CRUD is reached
- `Session.UserID` connects the logged-in user to `Emp_details.CreatedBy`

## The six conflicts and how they were resolved

### 1. Different namespaces

The login project used `Login_and_Register`, while EmployeeDetails used `EmployeeDetails`.

The three imported forms and their Designer files were changed to the host namespace:

`EmployeeDetails`

The root namespace was intentionally left as `EmployeeDetails`, as required by the lab instructions.

### 2. Different data providers

The login application originally used `System.Data.OleDb` and an Access database.

The merged application uses `System.Data.SqlClient` for all database operations. `User.cs` contains parameterized SQL using named parameters such as `@Username` and `@Password`.

There is no OleDb connection in the merged application.

### 3. Two databases

The Access database and the original EmployeeDetails database were replaced by one SQL Server database:

`dbCompanyApp`

It contains:

- `dbo.Users`
- `dbo.Emp_details`

The complete schema is included in `Schema.sql`.

### 4. Different framework versions

The login application targeted .NET Framework 4.7.2.

The EmployeeDetails host targeted .NET Framework 4.8, so the merged application remains on:

`.NET Framework 4.8`

This avoids downgrading the host project.

### 5. Two Program.cs / Main() methods

Only the host `Program.cs` was kept.

The final entry point is:

`Application.Run(new frmLogin());`

The imported login `Program.cs` was not added. Therefore the executable has one entry point.

### 6. Hidden Access file dependency

The original login project depended on `db_users.mdb` under `bin\Debug`.

That dependency was removed. Users are now stored in `dbo.Users` in SQL Server, so cleaning `bin` does not remove the login database.

## Unified database design

`dbo.Users` uses an integer identity key:

- `UserID` — primary key
- `Username` — unique username
- `Password` — stored password field
- `CreatedAt` — account creation timestamp

`dbo.Emp_details` contains:

- employee information
- `CreatedBy` — nullable foreign key to `Users.UserID`

`CreatedBy` is nullable because employees migrated from the original CRUD database may not have a known creator.

### Access account migration

The old Access accounts must be inserted into `dbo.Users` using:

```sql
INSERT INTO dbo.Users (Username, Password)
VALUES ('username', 'password');
```

`UserID` is intentionally omitted so SQL Server can generate the identity value.

## The three-file form rule

Each imported WinForms form was moved with its three associated files:

- `frmLogin.cs`
- `frmLogin.Designer.cs`
- `frmLogin.resx`

The same rule was followed for `frmRegister` and `frmDashboard`.

Only the `.cs` files were added through Visual Studio's Existing Item dialog; the Designer and resource files were kept beside their form files so Visual Studio could nest them.

No second `.csproj`, `Program.cs`, `App.config`, or `Properties` folder was imported.

## OleDb to SqlClient migration

The login data access was rewritten in `User.cs`.

The old:

- `OleDbConnection`
- `OleDbCommand`
- Access connection string
- concatenated SQL
- positional `?` parameters

were replaced by:

- `SqlConnection`
- `SqlCommand`
- SQL Server connection string from `App.config`
- parameterized SQL
- named parameters such as `@Username`

`ValidateLogin()` returns the matching `UserID`, not only a Boolean. This ID is stored in `Session.UserID`.

## Session

`Session.cs` contains:

- `Session.UserID`
- `Session.Username`
- `Session.Clear()`

The login form sets these values after successful authentication.

Employee creation then uses:

```csharp
employee.CreatedBy = Session.UserID;
```

## Application flow

```text
Program
  |
  v
frmLogin
  | successful login
  v
frmDashboard
  |
  +--> Manage Employees --> frmEmployee
  |
  +--> Logout --> new frmLogin
```

Registration is available from the login screen.

Logout confirms the action, calls `Session.Clear()`, shows a new login form, and closes the dashboard instead of calling `Application.Exit()`.

The login form also handles `FormClosed` so closing the login window exits the application.

## CreatedBy and LEFT JOIN

Employee records are linked to the user who created them through:

`Emp_details.CreatedBy -> Users.UserID`

The grid uses:

```sql
SELECT
    e.EmpId,
    e.EmpName,
    e.EmpAge,
    e.EmpContact,
    e.EmpGender,
    u.Username AS CreatedBy
FROM dbo.Emp_details e
LEFT JOIN dbo.Users u
    ON e.CreatedBy = u.UserID;
```

A `LEFT JOIN` is required instead of an inner `JOIN` because migrated employee rows can have `CreatedBy = NULL`. With an inner join, those older employees would disappear from the grid. With a left join, all employees remain visible and the creator is shown when a matching user exists.

## Schema-change handling

The DataGridView is bound to the column names returned by the SQL query. The row-selection code uses names such as:

- `EmpId`
- `EmpName`
- `EmpAge`
- `EmpContact`
- `EmpGender`

instead of depending on fixed numeric column positions.

The creator is returned as the named column `CreatedBy`.

## Build error and fix

During a merge, a common build error is `CS0017`: more than one entry point is defined. This occurs when the login project's `Program.cs` is copied into the EmployeeDetails project in addition to the existing `Program.cs`.

The fix is to keep only one `Program.cs` and use:

```csharp
Application.Run(new frmLogin());
```

The imported login `Program.cs` is therefore excluded from the final project.

## Why one database is better than two

One database gives the application one consistent source of truth for authentication and employee data. It also allows a real foreign-key relationship between the logged-in user and the employee record they create. The `CreatedBy` field connects the two former applications through `Users.UserID`. The `LEFT JOIN` then allows the employee grid to show the creator's username while still displaying migrated employees whose creator is unknown. Keeping both applications on separate databases would make this relationship fragile and would require unnecessary synchronization.

## Screenshots required for submission

Add the following screenshots to the final report/README evidence:

1. SSMS Object Explorer showing `dbCompanyApp`, `dbo.Users`, and `dbo.Emp_details`.
2. `SELECT * FROM dbo.Users` showing migrated Access accounts.
3. Visual Studio Solution Explorer showing the three nested form files.
4. Login screen.
5. Registration screen.
6. Dashboard.
7. Employee CRUD screen.
8. Employee grid showing the `CreatedBy` username.
9. Logout returning to a fresh login screen.

## Files

- `Schema.sql` — unified SQL Server schema
- `.gitignore` — excludes `bin`, `obj`, `.vs`, and Access database files
- `25-60783-1_CompanyApp.sln` — Visual Studio solution
- `25-60783-1_CompanyApp/` — WinForms project
