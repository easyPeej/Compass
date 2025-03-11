namespace Compass.ViewModels

open Compass.Database.Reports
open Compass.Services
open ReactiveUI

type ReportsViewModel() =
    inherit ReactiveObject()
    
    let mutable fullReport = []
    let mutable reportById = []
    let mutable assigned_staff = 0L

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
        
    member this.LoadAllReports() =
        let fetchedReports = FetchReports()
        this.FullReport <- fetchedReports
        
    member this.FetchReportsByAssigned () =
        printf ($"Fetching reports for user ID: {assigned_staff}") // Debugging
        let fetchedReportsById = FetchReportsByAssigned assigned_staff
        printf ($"Fetched {fetchedReportsById.Length} reports.") // Debugging
        this.ReportById <- fetchedReportsById
    
        
        
    
    
    
    