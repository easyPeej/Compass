namespace Compass.Views

open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.Markup.Xaml
open Compass.Database.Reports
open Compass.ViewModels


type ViewChildDataUC() as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()

    do
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.LoadAllReports()
        
        
    member this.ViewChildDataClick(sender: obj, e: RoutedEventArgs) =
        let button = sender :?> Button
        let childId = button.Tag :?> int
        printf $"Viewing child id: {childId}"
        