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
            permissions TEXT DEFAULT 'Standard');
            
            
                CREATE TABLE IF NOT EXISTS SafeguardingReports (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reporter_id INTEGER NOT NULL,
            concern_description TEXT NOT NULL,
            date_reported DATETIME DEFAULT CURRENT_TIMESTAMP,
            status TEXT CHECK(status IN ('Open', 'Under Review', 'Resolved')) NOT NULL DEFAULT 'Open',
            assigned_staff INTEGER DEFAULT NULL,
            child_id INTEGER NOT NULL,
            FOREIGN KEY (reporter_id) REFERENCES Users(id),
            FOREIGN KEY (assigned_staff) REFERENCES Users(id),
            FOREIGN KEY (child_id) REFERENCES Children(id)
            );
            
            
                CREATE TABLE IF NOT EXISTS Keywords (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            keyword TEXT UNIQUE NOT NULL
            );
            
            
                CREATE TABLE IF NOT EXISTS ReportKeywords (
            report_id INTEGER NOT NULL,
            keyword_id INTEGER NOT NULL,
            PRIMARY KEY (report_id, keyword_id),
            FOREIGN KEY (report_id) REFERENCES SafeguardingReports(id),
            FOREIGN KEY (keyword_id) REFERENCES Keywords(id)
            );
            
            
                CREATE TABLE IF NOT EXISTS Children (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            first_name TEXT NOT NULL,
            last_name TEXT NOT NULL,
            date_of_birth DATE NOT NULL,
            gender TEXT CHECK(gender IN ('Male', 'Female', 'Other')) NOT NULL,
            address TEXT NOT NULL,
            school TEXT,
            emergency_contact TEXT NOT NULL,
            social_worker TEXT,
            risk_level TEXT CHECK(risk_level IN ('Low', 'Medium', 'High')) NOT NULL DEFAULT 'Low',
            medical_notes TEXT,
            safeguarding_notes TEXT
            );
            "
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
        
        

            
            
            
        // Checking if children exist, if not, insert dummy children
        let childCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Children")
        
        if childCount = 0 then
            let insertChildQuery = "
                INSERT INTO Children (first_name, last_name, date_of_birth, gender, address, school, emergency_contact, social_worker, risk_level, medical_notes, safeguarding_notes)
                VALUES (@FirstName, @LastName, @DateOfBirth, @Gender, @Address, @School, @EmergencyContact, @SocialWorker, @RiskLevel, @MedicalNotes, @SafeguardingNotes)"
        
            let children = [
                {| FirstName = "Jake"; LastName = "Williams"; DateOfBirth = "2010-08-15"; Gender = "Male"; Address = "123 Oak St"; School = "Hilltop Primary"; EmergencyContact = "Mary Smith - 555-1111"; SocialWorker = "John Doe"; RiskLevel = "Medium"; MedicalNotes = "Asthma"; SafeguardingNotes = "Parental neglect observed." |}
                {| FirstName = "Sophie"; LastName = "Davis"; DateOfBirth = "2012-11-22"; Gender = "Female"; Address = "456 Maple Ave"; School = "Sunrise Academy"; EmergencyContact = "David Brown - 555-2222"; SocialWorker = "Anna Green"; RiskLevel = "High"; MedicalNotes = "Severe allergies"; SafeguardingNotes = "Previous reports of domestic violence." |}
            ]
        
            connection.Execute(insertChildQuery, children) |> ignore
            printfn "Dummy children inserted."
        else
            printfn "Children already exist, skipping insert."
        
        
        // Checking if keywords exist, if not, insert dummy keywords
        let keywordCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Keywords")
        
        if keywordCount = 0 then
            let insertKeywordQuery = "INSERT INTO Keywords (keyword) VALUES (@Keyword)"
        
            let keywords = [
                {| Keyword = "Abuse" |}
                {| Keyword = "Neglect" |}
                {| Keyword = "Grooming" |}
                {| Keyword = "Bullying" |}
                {| Keyword = "Self-harm" |}
                {| Keyword = "Depression" |}
                {| Keyword = "Poverty" |}
                {| Keyword = "Cyberbullying" |}
                {| Keyword = "Substance misuse" |}
            ]
        
            connection.Execute(insertKeywordQuery, keywords) |> ignore
            printfn "Dummy keywords inserted."
        else
            printfn "Keywords already exist, skipping insert."
        
        
        // Checking if ReportKeywords exist, if not, insert dummy associations
        let reportKeywordCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM ReportKeywords")
        
        if reportKeywordCount = 0 then
            let insertReportKeywordQuery = "INSERT INTO ReportKeywords (report_id, keyword_id) VALUES (@ReportId, @KeywordId)"
        
            let reportKeywords = [
                {| ReportId = 1; KeywordId = 1 |} // Abuse
                {| ReportId = 1; KeywordId = 5 |} // Self-harm
                {| ReportId = 2; KeywordId = 4 |} // Bullying
            ]
        
            connection.Execute(insertReportKeywordQuery, reportKeywords) |> ignore
            printfn "Dummy report-keyword associations inserted."
        else
            printfn "ReportKeywords already exist, skipping insert."
            
            
            
                // Inserting dummy safeguarding data
        let sgCount: int = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM SafeguardingReports")
        
        if sgCount = 0 then
            
            let insertReportQuery = "INSERT INTO SafeguardingReports (reporter_id, concern_description, status, assigned_staff, child_id)
                VALUES (@ReporterId, @ConcernDescription, @Status, @AssignedStaff, @ChildId)"
                
            let reports = [
                {| ReporterId = 1; ConcernDescription = "Student showing signs of distress."; Status = "Open"; AssignedStaff = 2; ChildId = 1 |}
                {| ReporterId = 2; ConcernDescription = "Incident of bullying reported."; Status = "Under Review"; AssignedStaff = 1; ChildId = 2 |}
            ]

            connection.Execute(insertReportQuery, reports) |> ignore


