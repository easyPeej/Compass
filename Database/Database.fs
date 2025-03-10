namespace Compass.Database

open Dapper
open Microsoft.Data.Sqlite


//////////////// LIKELY TO DELETE THIS AS SCHEMA SHOULD BE IN PLACE /////////////////////////////////

module Database =
    
    let initializeDatabase () =
        use connection = new SqliteConnection("Data Source=Data.db")
        connection.Open()
        
        // Creates a table for users if there isn't one already - purely for testing 
        use createTableCommand = connection.CreateCommand()
        createTableCommand.CommandText <- "
            CREATE TABLE IF NOT EXISTS Users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            first_name TEXT NOT NULL,
            last_name TEXT NOT NULL,
            email TEXT UNIQUE NOT NULL,
            password_hash TEXT NOT NULL,
            role TEXT CHECK(role IN ('Teacher', 'Admin', 'Safeguarding Lead', 'Support Staff')) NOT NULL,
            phone TEXT DEFAULT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            last_login DATETIME DEFAULT NULL,
            status TEXT CHECK(status IN ('Active', 'Suspended', 'Pending')) NOT NULL DEFAULT 'Active',
            permissions TEXT DEFAULT 'Standard')"
        createTableCommand.ExecuteNonQuery() |> ignore
        
       
        createTableCommand.CommandText <- "
            CREATE TABLE IF NOT EXISTS SafeguardingReports (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reporter_id INTEGER NOT NULL,
            concern_description TEXT NOT NULL,
            date_reported DATETIME DEFAULT CURRENT_TIMESTAMP,
            status TEXT CHECK(status IN ('Open', 'Under Review', 'Resolved')) NOT NULL DEFAULT 'Open',
            assigned_staff INTEGER DEFAULT NULL,
            FOREIGN KEY (reporter_id) REFERENCES Users(id),
            FOREIGN KEY (assigned_staff) REFERENCES Users(id)
            )"
        createTableCommand.ExecuteNonQuery() |> ignore
        
    
        // Checking if users currently in DB if not it inputs some dummy data - purely for testing
        let userCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Users")
        
        if userCount = 0 then
    
            // FYI these queries are parameterized through dapper securing them against SQL injections
            let insertQuery = "
                INSERT INTO Users (first_name, last_name, email, password_hash, role, phone, status, permissions)
                VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role, @Phone, @Status, @Permissions)"
            
            let users = [
                {| FirstName = "Alice"; LastName = "Johnson"; Email = "alice@example.com"; PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"); Role = "Teacher"; Phone = "1234567890"; Status = "Active"; Permissions = "Standard";  |}
                {| FirstName = "Bob"; LastName = "Smith"; Email = "bob@example.com"; PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminpass"); Role = "Admin"; Phone = "0987654321"; Status = "Active"; Permissions = "Full";  |}
            ]
        
            
            connection.Execute(insertQuery, users) |> ignore
            printfn "Default users inserted."
        else
            printfn "Users already exist, skipping insert."
        
        
        

        // Inserting dummy safeguarding data
        let sgCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SafeguardingReports")
        
        if sgCount = 0 then
            let insertReportQuery = "INSERT INTO SafeguardingReports (reporter_id, concern_description, status, assigned_staff)
                VALUES (@ReporterId, @ConcernDescription, @Status, @AssignedStaff)"
                
            let reports = [
                {| ReporterId = 1; ConcernDescription = "Student showing signs of distress."; Status = "Open"; AssignedStaff = 2 |}
                {| ReporterId = 2; ConcernDescription = "Incident of bullying reported."; Status = "Under Review"; AssignedStaff = 1 |}
            ]

            connection.Execute(insertReportQuery, reports) |> ignore