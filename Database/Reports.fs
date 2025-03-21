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
            
        let FetchChildrenNames() =
            use connection = DbConnect.GetConnection()
            let query = "SELECT first_name || ' ' || last_name FROM Children"
            connection.Query<string>(query)
            |> Seq.toList
        
        // WIP    
        let FetchByChildId(id) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT first_name || ' ' || last_name FROM Children WHERE Id = @Id"
            let parameters =
                {|
                  Id = id
                  |}
            
            connection.Query<string>(query, parameters)
            |> Seq.toList
            
            
        let CreateNewReport(reporterId, concernDescription, status, assignedStaffId, childId) =
            use connection = DbConnect.GetConnection()
            let query = "INSERT INTO SafeguardingReports (
            reporter_id, concern_description, status, assigned_staff, child_id)
            VALUES (@ReporterId, @ConcernDescription, @Status, @AssignedStaff, @ChildId)"
            
            let parameters =
                {|
                    ReporterId =  reporterId
                    ConcernDescription = concernDescription
                    Status = status
                    AssignedStaff = assignedStaffId
                    ChildId = childId
                |}
                
            connection.Execute(query, parameters)
            
            
        let FetchChildData(id) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM Children WHERE id = @Id"
            
            let parameters =
                {| Id = id |}
            
            connection.Query<ChildData>(query, parameters)
            |> Seq.toList