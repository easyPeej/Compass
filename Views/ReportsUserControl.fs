namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels


type ReportsUserControl() as this =
    inherit UserControl()

    do
        AvaloniaXamlLoader.Load(this)
        let viewModel = ReportsViewModel()
        this.DataContext <- viewModel
        viewModel.LoadAllReports()
        
        