namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.Services
open Compass.ViewModels

type BannerTemplate() as this =
    inherit UserControl()
    
    do
        AvaloniaXamlLoader.Load(this)
        
        match UserSession.UserSession with
        | Some user -> this.DataContext <- DashboardViewModel()
        | None -> this.DataContext <- null
        
    
    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)

    