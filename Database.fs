namespace Compass
open Dapper
open Microsoft.Data.Sqlite


//////////////// LIKELY TO DELETE THIS AS SCHEMA SHOULD BE IN PLACE /////////////////////////////////

module Database =
    
    let initializeDatabase () =
        use connection = new SqliteConnection("Data Source=Data.db")
        connection.Open()
        
        // Creates a table if there isn't one already - purely for testing 
        use createTableCommand = connection.CreateCommand()
        createTableCommand.CommandText <- "
            CREATE TABLE IF NOT EXISTS Users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            first_name TEXT NOT NULL,
            last_name TEXT NOT NULL,
            email TEXT UNIQUE NOT NULL,
            password_hash TEXT NOT NULL,
            role TEXT CHECK(role IN ('Teacher', 'Admin', 'Safeguarding Lead', 'Support Staff', 'Parent', 'IT Staff')) NOT NULL,
            phone TEXT DEFAULT NULL,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            last_login DATETIME DEFAULT NULL,
            status TEXT CHECK(status IN ('Active', 'Suspended', 'Pending')) NOT NULL DEFAULT 'Active',
            permissions TEXT DEFAULT 'Standard'
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