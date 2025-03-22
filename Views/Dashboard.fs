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
        
        // For ReportsUserControl
        let reportsControl = new ReportsUC()
        let Panel = this.FindControl<StackPanel>("ContainerPanel")
        Panel.Children.Add(reportsControl)
    

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
