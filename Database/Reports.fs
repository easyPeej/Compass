namespace Compass.Database

open Dapper
open Microsoft.Data.Sqlite
open Compass.Models

module Reports =
        
        let FetchReports () =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports"
            connection.Query<SafeguardingReports>(query)
            |> Seq.toList
