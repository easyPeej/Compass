namespace Compass.Views 

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml

type BannerTemplate() as this =
    inherit UserControl()
    
    do AvaloniaXamlLoader.Load(this)
    
    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)

    