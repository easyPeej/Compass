namespace Compass.Views  

open Compass.Services
open Compass.ViewModels
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Interactivity

type UpdateReport(reportId: int) as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()
    

    do 
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.SingleReportId <- reportId
        viewModel.FetchSingleReport reportId
        
        
    member this.UpdateReportMethod(sender: obj, e: RoutedEventArgs) =
        
        viewModel.UpdateReport()

        

    

