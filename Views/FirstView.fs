namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.Views
open Avalonia.Interactivity

type FirstView() as this =
    inherit UserControl()

    do this.InitializeComponent()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)

