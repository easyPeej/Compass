namespace Compass.Views

open System.Collections.ObjectModel
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Interactivity
open Avalonia.Markup.Xaml
open Compass.Database.Reports
open Compass.Models
open Compass.ViewModels
open Microsoft.FSharp.Core


type ViewChildDataUC(childId: int) as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()

    do
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.FetchChildDataById(childId)
        
    member val ChildDataById = ObservableCollection<ChildDataModel>() with get, set