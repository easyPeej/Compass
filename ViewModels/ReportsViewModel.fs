namespace Compass.ViewModels

open Compass.Database
open Compass.Database.Reports
open Compass.Database.Staff
open Compass.Services
open ReactiveUI
open System.Collections.ObjectModel
open Compass.Models
open Dapper

type ReportsViewModel() as this =
    inherit ReactiveObject()
    
    // Read reports 
    let mutable fullReport = ObservableCollection<SafeguardingReports>()
    let mutable reportById = ObservableCollection<SafeguardingReports>()
    let mutable assigned_staff = 0L
    let mutable selectedReport: SafeguardingReports option = None
    let mutable status = ""
    let mutable childInformation = ObservableCollection<ChildData>()
    
    // for display / creating reports
    let mutable allKeywords = ObservableCollection<string>()
    let mutable selectedKeyword: string option = None
    let mutable childrenNames = ObservableCollection<string>()
    let mutable childName = ObservableCollection<string>()
    let mutable staffNames = ObservableCollection<string>()
    let mutable reportConcernText = ""
    
    let mutable selectedChildId: int64 option = None


    //let mutable count = 0
    
    do
        this.FetchKeywords()
        this.FetchChildren()
        this.FetchStaff()
        
       
        match UserSession.UserSession with
        | Some user ->
             assigned_staff <- user.id
        | None ->
            assigned_staff <- 0L
            
    
    member this.FullReport
        with get() = fullReport
        and set value =
            fullReport <- value
            this.RaisePropertyChanged()
            
    member this.ReportById
        with get() = reportById
        and set value =
            reportById <- value
            this.RaisePropertyChanged()
            
            
    member this.SelectedReport
        with get() = selectedReport
        and set value =
            selectedReport <- value
            this.RaisePropertyChanged()

    member this.StatusOptions = ["Pending"; "In Progress"; "Resolved"]
    
    member this.Status
        with get() = status
        and set value =
            status <- value
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
            
    member this.SelectedChildId
        with get() = selectedChildId
        and set value =
            selectedChildId <- value
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
            
            
            
    // Methods bellow         
    
    member this.LoadAllReports() =
        let fetchedReports = FetchReports()
        this.FullReport <- ObservableCollection<SafeguardingReports>(fetchedReports |> List.toSeq)
        
    member this.FetchReportsByAssigned () =
        printf $"Fetching reports for user ID: {assigned_staff}" // Debugging
        let fetchedReportsById = FetchReportsByAssigned assigned_staff
        printf $"Fetched {fetchedReportsById.Length} reports." // Debugging
        this.ReportById <- ObservableCollection<SafeguardingReports>(fetchedReportsById |> List.toSeq)
        
    (*member this.SaveReport()=
        match this.SelectedReport with
        | Some report ->
            use connection = DbConnect.GetConnection()
            let query = "UPDATE Safeguarding SafeguardingReports SET concern_description = @ConcernDescription, status = @Status WHERE id = @id "
            connection.Execute(query, report) |> ignore
        | None -> printf "No report selected for update"*)
        
    member this.FetchKeywords() =
        let fetchedKeywords = FetchAllKeywords()
        this.AllKeywords <- ObservableCollection<string>(fetchedKeywords |> List.toSeq)
        
        
    member this.FetchChildren() =
        let fetchChildrenNames = FetchChildrenNames()
        this.ChildrenNames <- ObservableCollection<string>(fetchChildrenNames |> List.toSeq)
        
    // WIP    
    member this.FetchChildNameById() =
        let childNameId = FetchByChildId()
        // unsure about type 
        this.ChildNameById <- ObservableCollection<string>(childNameId |> List.toSeq)
        
    member this.FetchStaff() =
        let staffNames = FetchStaff()
        this.StaffNames <- ObservableCollection<string>(staffNames |> List.toSeq)
        
        
    // Testing 
    member this.ChildDataById
        with get() = childInformation
        and set value =
            childInformation <- value
            this.RaisePropertyChanged()
       
    member this.FetchChildDataById(id) =
        let data = FetchChildData id
        this.ChildDataById <- ObservableCollection<ChildData>(data |> List.toSeq)
        
        
    // Testing 
        
        
    (*
    member this.CreateReport() =
        let createReport = CreateNewReport(assigned_staff, )
        this.LoadAllReports()
        *)
        
    (*member this.fetchChildData() =
        let childData = *)
