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
        let reportsControl = new ReportsUserControl()
        let container = this.FindControl<StackPanel>("ContainerPanel")
        container.Children.Add(reportsControl)
    

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
