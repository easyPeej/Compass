namespace Compass.Database

open Compass.Services
open Dapper
open Compass.Models



module Reports =
        
        // Fetches all reports - no parameters
        let FetchReports () =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports"
            connection.Query<SafeguardingReports>(query)
            |> Seq.toList
            
        let CountReports() =
            use connection = DbConnect.GetConnection()
            let query = "SELECT COUNT(*) FROM SafeguardingReports"
            connection.ExecuteScalar<int>(query)
            
        let CountReportsByStatus(status: string) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT COUNT(*) FROM SafeguardingReports WHERE status = @Status"
            connection.ExecuteScalar<int>(query, {| Status = status |})
        
        // Fetches assigned reports - parameter assigned_staff (their id no.)    
        let FetchReportsByAssigned (assigned_staff) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports WHERE assigned_staff = @assigned_staff"
            connection.Query<SafeguardingReports>(query, {| assigned_staff = assigned_staff |})
            |> Seq.toList
        
        // fetches single report via its id number    
        let FetchSingleReport(id) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM SafeguardingReports WHERE id = @Id"
            connection.QueryFirstOrDefault<SafeguardingReports>(query, {| Id = id |})
            
            
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
            connection.Query<string>(query, {| Id = id |})
            |> Seq.toList
            
        
        // WIP    
        let CreateNewReport(reporterId, concernDescription, status, assignedStaffId, childId) =
            use connection = DbConnect.GetConnection()
            let command = "INSERT INTO SafeguardingReports (
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
                
            connection.Execute(command, parameters)
            
            
        let FetchChildData(id) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT * FROM Children WHERE id = @Id"            
            connection.Query<ChildDataModel>(query, {| Id = id |})
            |> Seq.toList
            
        let FetchChildIdByName(firstName: string) (lastName: string) =
            use connection = DbConnect.GetConnection()
            let query = "SELECT id FROM Children WHERE first_name = @FirstName AND last_name = @LastName"
            
            let parameters =
                {|
                  FirstName = firstName
                  LastName = lastName
                |}
            connection.Query<ChildDataModel>(query, parameters)
            |> Seq.toList
        
        let FetchSelectedStaffId (first) (last) :int64 option=
            use connection = DbConnect.GetConnection()
            let query = "SELECT id FROM Users WHERE first_name = @First AND last_name = @Last"
            let result = connection.Query<int64>(query, {| First = first
                                                           Last = last |}) |> Seq.tryHead
            match result with
            | Some(id) -> Some(id) 
            | None -> None 
        
        
        let ExecuteUpdateReport (report: SafeguardingReports) (currentUserID: int64) =
            let extractedId =
                match report.assigned_staff with
                | Some(idNum) -> idNum
                | None -> 0

            let connection = DbConnect.GetConnection()
            let query =
                "UPDATE SafeguardingReports SET reporter_id = @ReporterID, concern_description = @ConcernDescription,
                status = @Status, severity = @Severity, date_reported = @Date, assigned_staff = @AssignedStaffId WHERE id = @ReportId" 
                        

            let parameters =
                dict [
                    "ReportId", box (int report.id)
                    "ReporterID", box (int currentUserID)
                    "ConcernDescription", box report.concern_description
                    "Date", box System.DateTime.Now
                    "Status", box report.status
                    "AssignedStaffId", box (int extractedId)
                    "Severity", box report.severity
                ]
                
            printfn $"{parameters}"
                
            connection.Execute(query, parameters) |> ignore
