namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Interactivity
open Compass.Database.Staff
open Compass.Services
open Compass.ViewModels


type ViewReportsAssignedUC() as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()
    

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        
        let userId = UserSession.GetUserId
        viewModel.FetchReportsByAssigned()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
        printfn $"{FetchAllStaffDetails}"

    member this.LoadChildData(sender: obj, e: RoutedEventArgs) =
        let button = sender :?> Button
        match button.Tag with
        | :? int as childId ->
            let childView = new ChildInfoUC(childId)
            this.NavigateToPage(childView)
        | _ -> printfn "invalid child id"
        
        
        
        member this.LoadUpdateReport(sender: obj, e: RoutedEventArgs) =
        let button = sender :?> Button
        match button.Tag with
        | :? int as reportId ->
            let updateReportView = new UpdateReport(reportId)
            this.NavigateToPage(updateReportView)
        | _ -> printfn "invalid report id"    
        

    member this.NavigateToPage(page: UserControl) =
        match this.VisualRoot with
        | :? Window as mainWindow -> 
            match mainWindow.FindControl<ContentControl>("MainContent") with
            | null -> ()
            | contentControl -> 
                contentControl.Content <- page
        | _ -> () 