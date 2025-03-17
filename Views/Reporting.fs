namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels


type Reporting() as this =
    inherit UserControl()

    do
        this.InitializeComponent()
        this.DataContext <- ReportsViewModel()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
