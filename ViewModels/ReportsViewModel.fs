namespace Compass.ViewModels

open Compass.Database
open Compass.Database.Reports
open Compass.Services
open ReactiveUI
open System.Collections.ObjectModel
open Compass.Models
open Dapper

type ReportsViewModel() =
    inherit ReactiveObject()
    
    let mutable fullReport = ObservableCollection<SafeguardingReports>()
    let mutable reportById = ObservableCollection<SafeguardingReports>()
    let mutable assigned_staff = 0L
    
    let mutable selectedReport: SafeguardingReports option = None
    let mutable status = ""


    //let mutable count = 0
    
    do
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
            
    member this.LoadAllReports() =
        let fetchedReports = FetchReports()
        this.FullReport <- ObservableCollection<SafeguardingReports>(fetchedReports |> List.toSeq)
        
    member this.FetchReportsByAssigned () =
        printf $"Fetching reports for user ID: {assigned_staff}" // Debugging
        let fetchedReportsById = FetchReportsByAssigned assigned_staff
        printf $"Fetched {fetchedReportsById.Length} reports." // Debugging
        this.ReportById <- ObservableCollection<SafeguardingReports>(fetchedReportsById |> List.toSeq)
        
    member this.SaveReport()=
        match this.SelectedReport with
        | Some report ->
            use connection = DbConnect.GetConnection()
            let query = "UPDATE Safeguarding SafeguardingReports SET concern_description = @ConcernDescription, status = @Status WHERE id = @id "
            connection.Execute(query, report) |> ignore
        | None -> printf "No report selected for update"
            


