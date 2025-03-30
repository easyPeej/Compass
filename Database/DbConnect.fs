namespace Compass.Database

open Dapper
open Microsoft.Data.Sqlite

module DbConnect =
    
    // set variable to avoid hardcoding source inside the function 
    let private connString = "Data Source=Data.db"
    
    // opens our database
    let GetConnection() =
        try
            let connection = new SqliteConnection(connString)
            connection.Open()
            connection
        with
        | ex ->
            printfn $"Database connection failed: %s{ex.Message}"
            raise ex
