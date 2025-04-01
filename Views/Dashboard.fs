namespace Compass.Views 

open System.Collections.Generic
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels


type Dashboard() as this =
    inherit UserControl()
    
    let viewModel = DashboardViewModel()

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        
        match viewModel.Role with
        | "Admin" -> 
                let viewUsers = new ViewUsersUC()
                let Panel = this.FindControl<StackPanel>("ContainerPanel")
                Panel.Children.Add(viewUsers)
        | "Teacher" -> 
                let ViewAssignedReports = new ViewReportsAssignedUC()
                let Panel = this.FindControl<StackPanel>("ContainerPanel")
                Panel.Children.Add(ViewAssignedReports)
        | "Safeguarding Lead" ->
                let viewAllReports = new ReportsUC()
                let Panel = this.FindControl<StackPanel>("ContainerPanel")
                Panel.Children.Add(viewAllReports)                
        | "Support Staff" ->
                let ViewAssignedReports = new ViewReportsAssignedUC()
                let Panel = this.FindControl<StackPanel>("ContainerPanel")
                Panel.Children.Add(ViewAssignedReports)            
        | _ -> printfn "none"
        
        
        (*// views report UC
        let reportsControl = new ReportsUC()
        let Panel = this.FindControl<StackPanel>("ContainerPanel")
        Panel.Children.Add(reportsControl)*)
        
        // view total open cases
        let test = new ReportCountUC()
        let Panel2 = this.FindControl<StackPanel>("ContainerPanel2")
        Panel2.Children.Add(test)
    

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)


