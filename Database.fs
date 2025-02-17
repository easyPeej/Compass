namespace Compass
open System
open Microsoft.Data.Sqlite

module Database =
    
    (* This let block is an unfinished attempt at storing the database in a more secure location eg AppData - only accessible via administrative users. *)
    
    (*let dbPath=
        let basePath =
            if OperatingSystem.IsWindows() then
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            else 
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share")
        Path.Combine(basePath, "Compass", "app.db")
            
    let connectionString = $"Data Source={dbPath}"    *)    
    
    
    let initializeDatabase () =
        use connection = new SqliteConnection("Data Source=Data.db")
        connection.Open()
        use command = connection.CreateCommand()
        command.CommandText <- "
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            )"
        command.ExecuteNonQuery() |> ignore