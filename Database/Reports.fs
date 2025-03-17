namespace Compass.Database

open Dapper
open Compass.Models



module Reports =
        
        // Fetches all reports - no parameters
        let FetchReports () =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports"
            connection.Query<SafeguardingReports>(query)
            |> Seq.toList
        
        // Fetches assigned reports - parameter assigned_staff (their id no.)    
        let FetchReportsByAssigned (assigned_staff) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports WHERE assigned_staff = @assigned_staff"
            connection.Query<SafeguardingReports>(query, {| assigned_staff = assigned_staff |})
            |> Seq.toList
            
        let FetchAllKeywords () =
            use connection = DbConnect.GetConnection()
            let query = "SELECT keyword FROM Keywords"
            connection.Query<string>(query)
            |> Seq.toList 
            
