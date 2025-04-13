namespace Compass.Services

open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia
open System

module Navigation = 

    let NavigateTo (page: UserControl) =
        let mainWindow = Application.Current.ApplicationLifetime
                        |> function
                            | :? IClassicDesktopStyleApplicationLifetime as desktop ->
                                desktop.MainWindow
                            | _ -> null
                            
        match mainWindow with
        | :? Window as window ->
            match window.FindControl<ContentControl>("MainContent") with
            | null -> ()
            | contentControl -> contentControl.Content <- page
        | _ -> ()