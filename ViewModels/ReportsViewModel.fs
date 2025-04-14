namespace Compass.ViewModels

open Compass.Database.Reports
open Compass.Database.Staff
open Compass.Services
open ReactiveUI
open System.Collections.ObjectModel
open Compass.Models


type ReportsViewModel() as this =
    inherit ReactiveObject()
    
    
    // Read reports 
    let mutable fullReport = ObservableCollection<SafeguardingReports>()
    let mutable reportCount = 0
    let mutable reportCountByStatus = 0
    
    // assigned staff id - not report id
    let mutable reportById = ObservableCollection<SafeguardingReports>()
    
    // current session user id to find reports assigned to them 
    let mutable currentSessionUserId = 0L
    
    // to view child data in ChildInfoUC
    let mutable childInformation = ObservableCollection<ChildDataModel>()
     
    

    // for display / creating reports
    let mutable allKeywords = ObservableCollection<string>()
    let mutable selectedKeyword: string option = None
    let mutable childrenNames = ObservableCollection<string>()
    let mutable childName = ObservableCollection<string>()
    let mutable staffNames = ObservableCollection<string>()
    let mutable reportConcernText = ""
    let mutable currentUserId = 0L
    let mutable roleIsLead = false
    
    
    
    // everything here is for updating a report
    //let mutable singleReport = ObservableCollection<SafeguardingReports>()
    let mutable singleReport: SafeguardingReports = 
        { id = 0
          reporter_id = 0
          concern_description = ""
          date_reported = System.DateTime.Now
          status = ""
          assigned_staff = None
          child_id = 0
          severity = ""
          raised_to_social_care = false }
        
    let mutable singleReportId= 0L
    let mutable staffName = ""
    let mutable severityOption = ""
    let mutable keywords = []
    let mutable statusOption = ""
    let mutable raisedToSSOption: bool = false
    let mutable reportString = ""
    let mutable assignedStaffId:int64 option = None
    // -----------------------------------------
    
    // updating sg report 
    let mutable selectedChildName: string option = None
    (*let mutable selectedAssignedStaff = *)

    do
        this.FetchKeywords()
        this.FetchChildren()
        this.FetchStaff()
        this.CheckRole()
        
        match UserSession.UserSession with
        | Some user ->
             currentSessionUserId <- user.id
        | None ->
            currentSessionUserId <- 0L
   
    
    //////////////////////// GETTERS SETTERS ////////////////////////
    
    member this.StatusOptions = ["Open"; "Under Review"; "Resolved"]
    member this.SeverityDisplay = ["Low"; "Medium"; "High"]
    member this.KeywordsDisplay = ["Abuse"; "Neglect"; "Grooming"; "Bullying"; "Self-harm"; "Depression"; "Poverty"; "Cyberbullying"; "Substance misuse"]
    
             
    member this.UserId
        with get() =
            match UserSession.UserSession with
            | Some user -> user.id.ToString()
            | None -> "n/a"
            
    // all reports 
    member this.FullReport
        with get() = fullReport
        and set value =
            fullReport <- value
            this.RaisePropertyChanged()
            
    member this.TotalReportsCount
        with get() = reportCount
        and set value =
            reportCount <- value
            this.RaisePropertyChanged()
            
    member this.ReportOpenCount
        with get() = reportCountByStatus
        and set value =
            reportCountByStatus <- value
            this.RaisePropertyChanged()            
    member this.ReportResolvedCount
        with get() = reportCountByStatus
        and set value =
            reportCountByStatus <- value
            this.RaisePropertyChanged()            
    member this.ReportUnderReviewCount
        with get() = reportCountByStatus
        and set value =
            reportCountByStatus <- value
            this.RaisePropertyChanged()
        
    // update reports getters setters BEGINS -----------------------------
        
    member this.SingleReport
        with get() = singleReport
        and set value =
            singleReport <- value
            this.RaisePropertyChanged()
            
    member this.SingleReportId
        with get() = singleReportId
        and set value =
            singleReportId <- value
            this.RaisePropertyChanged()
            
    member this.ConcernDescription
        with get() = singleReport.concern_description
        and set value =
            singleReport <- {singleReport with concern_description = value}
            this.RaisePropertyChanged()
    member this.SelectedStatus
        with get() = singleReport.status
        and set value =
            singleReport <- {singleReport with status = value}
            this.RaisePropertyChanged()
    member this.SelectedSeverity
        with get() = singleReport.severity
        and set value =
            singleReport <- {singleReport with severity = value}
            this.RaisePropertyChanged()
    member this.SelectedAssignedStaff
        with get() = singleReport.assigned_staff
        and set value =
            singleReport <- {singleReport with assigned_staff = value}
            printfn $"this is the staff id: {singleReport.assigned_staff}"
            this.RaisePropertyChanged()
    
    // use staff name to find their id no for update report method     
    member this.StaffName                    
        with get() = staffName
        and set value =
            staffName <- value
            this.FetchStaffId()
            this.RaisePropertyChanged()
    
    // update reports getters setters ENDS -----------------------------
            
    
    
    // for reports assigned to the current user        
    member this.ReportByAssignedId
        with get() = reportById
        and set value =
            reportById <- value
            this.RaisePropertyChanged()
            
    member this.AllKeywords
        with get() = allKeywords
        and set value =
            if allKeywords <> value then
                allKeywords <- value 
                this.RaisePropertyChanged()
            
    member this.SelectedKeyword
        with get() = selectedKeyword
        and set value =
            selectedKeyword <- value
            this.RaisePropertyChanged()
            
            
    member this.SelectedChildName
        with get() =
            match selectedChildName with
            | Some name -> name
            | None -> ""
        and set value =
            selectedChildName <-
                match value with 
                | "" | null
                | _ -> Some value
            this.RaisePropertyChanged()
            
    member this.ChildrenNames
        with get() = childrenNames
        and set value =
            childrenNames <- value
            this.RaisePropertyChanged()
            
    member this.ChildNameById
        with get() = childName
        and set value =
            childName <- value
            this.RaisePropertyChanged()
            
    member this.StaffNames
        with get() = staffNames
        and set value =
            staffNames <- value
            this.RaisePropertyChanged()
            
    member this.ReportConcernText
        with get() = reportConcernText
        and set value =
            reportConcernText <- value
            this.RaisePropertyChanged()
            
    member this.ChildDataById
        with get() = childInformation
        and set value =
            childInformation <- value
            this.RaisePropertyChanged()
    
    member this.RoleIsLead
        with get() = roleIsLead
        and set value =
            roleIsLead <- value


    
            
    //////////////////////// METHODS ////////////////////////
    
    member this.LoadAllReports() =
        let fetchedReports = FetchReports()
        this.FullReport <- ObservableCollection<SafeguardingReports>(fetchedReports |> List.toSeq)
        
    member this.FetchReportCount() =
        let reportCountTotal = CountReports()
        this.TotalReportsCount <- reportCountTotal
        
    member this.FetchReportCountByStatus(status: string) =
        let reportByStatus = CountReportsByStatus status
        match status with
        | "Open" -> this.ReportOpenCount <- reportByStatus
        | "Resolved" -> this.ReportResolvedCount <- reportByStatus
        | "Under Review" -> this.ReportUnderReviewCount <- reportByStatus
        |_ -> printfn "incorrect status input"
        
    
    // gets data for the report we intend to update    
    member this.FetchSingleReport(id) =
        let singleReport = FetchSingleReport id
        this.SingleReport <- singleReport
        
    member this.FetchChildDataById(id: int) =
        let data = FetchChildData id
        this.ChildDataById <- ObservableCollection<ChildDataModel>(data |> List.toSeq)    
            
    member this.FetchReportsByAssigned () =
        let fetchedReportsById = FetchReportsByAssigned currentSessionUserId
        this.ReportByAssignedId <- ObservableCollection<SafeguardingReports>(fetchedReportsById |> List.toSeq)
        
        
    member this.FetchKeywords() =
        let fetchedKeywords = FetchAllKeywords()
        this.AllKeywords <- ObservableCollection<string>(fetchedKeywords |> List.toSeq)
        
        
    member this.FetchChildren() =
        let fetchChildrenNames = FetchChildrenNames()
        this.ChildrenNames <- ObservableCollection<string>(fetchChildrenNames |> List.toSeq)
          
    member this.FetchChildNameById() =
        let childNameId = FetchByChildId()
        // unsure about type 
        this.ChildNameById <- ObservableCollection<string>(childNameId |> List.toSeq)
        
    member this.FetchStaff() =
        let staffNames = FetchStaff()
        this.StaffNames <- ObservableCollection<string>(staffNames |> List.toSeq)
        
        
    member this.FetchStaffId () =
        let splitName = staffName.Split(' ')
        let first = splitName.[0]
        let last = splitName.[1]
        
        let staffId = FetchSelectedStaffId first last
        this.SelectedAssignedStaff <- staffId
        
    member this.UpdateReport () =
        ExecuteUpdateReport singleReport currentSessionUserId|> ignore
        

    member this.CheckRole() =
        match UserSession.GetRole() with
        | Some role when role = "Safeguarding Lead" ->
            this.RoleIsLead <- true
        | _ ->
            this.RoleIsLead <- false
        