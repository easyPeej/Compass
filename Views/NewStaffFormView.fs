namespace Compass.Views

open System
open Avalonia.ReactiveUI
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels

type NewStaffFormView() as this =
    inherit UserControl()
    
    // may not be needed
    let viewModel = NewStaffFormViewModel()
    
    do
        this.InitializeComponent()
        // also may not be needed
        this.DataContext <- viewModel
        

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)