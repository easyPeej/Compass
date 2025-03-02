namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.Views

type Dashboard() as this =
    inherit UserControl()

    do this.InitializeComponent()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
