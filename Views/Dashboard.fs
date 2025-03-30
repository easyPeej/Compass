namespace Compass.Views 

open System.Collections.Generic
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels


type Dashboard() as this =
    inherit UserControl()

    do
        this.InitializeComponent()
        this.DataContext <- DashboardViewModel()
        
        // views report UC
        let reportsControl = new ReportsUC()
        let Panel = this.FindControl<StackPanel>("ContainerPanel")
        Panel.Children.Add(reportsControl)
        
        // view total open cases
        let test = new ReportCountUC()
        let Panel2 = this.FindControl<StackPanel>("ContainerPanel2")
        Panel2.Children.Add(test)
    

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
