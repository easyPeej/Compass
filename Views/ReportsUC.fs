namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.Interactivity
open Avalonia.Markup.Xaml
open Compass.ViewModels


type ReportsUC() as this =
    inherit UserControl()
    
    let viewModel = ReportsViewModel()

    do
        AvaloniaXamlLoader.Load(this)
        this.DataContext <- viewModel
        viewModel.LoadAllReports()
        
        
    member this.LoadChildData(sender: obj, e: RoutedEventArgs) =
        let button = sender :?> Button
        match button.Tag with
        | :? int as childId ->
            let childView = new ViewChildDataUC(childId)
            this.Content <- childView
        | _ -> printfn "invalid child id"
        
        

