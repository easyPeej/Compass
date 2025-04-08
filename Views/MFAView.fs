namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels


type MFAView() as this =
    inherit UserControl()
    
    let viewModel = ""

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
    
        (*let inputCode = this.FindControl<TextBox>("InputCodeBox")
        (*inputCode.TextChanged.Add(fun _ -> viewModel.InputCode <- inputCode.Text)*)*)
        
        
        

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
    