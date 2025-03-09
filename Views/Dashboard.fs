namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml

open Compass.Models
open Compass.Services
open Compass.ViewModels


type Dashboard() as this =
    inherit UserControl()

    do
        this.InitializeComponent()
        this.DataContext <- DashboardViewModel()
    

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
