namespace Compass.Views


open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels
open Microsoft.FSharp.Core


type ChildInfoUC(childId: int) as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()

    do
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.FetchChildDataById(childId)
        
