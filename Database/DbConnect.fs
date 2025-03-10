namespace Compass.Database

open Dapper
open Microsoft.Data.Sqlite

module DbConnect =
    // opens our database
    let GetConnection() =
        let connection = new SqliteConnection("Data Source=Data.db")
        connection.Open()
        connection
    
    
   // closes our database - mostly redundant due to 'use' cleaning up when out of scope, but nice to have
    let CloseConnection(connection: SqliteConnection) =
        connection.Close()