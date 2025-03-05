namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels
open ReactiveUI

type LoginPage() as this =
    inherit UserControl()
    
    let viewModel = LoginViewModel()
    
    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        
        let viewModel = LoginViewModel()
    
        let emailTextBox = this.FindControl<TextBox>("EmailTextBox")
        let passwordBox = this.FindControl<TextBox>("PasswordBox")
        let loginButton = this.FindControl<Button>("LoginButton")
        let errorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock")
        
        // binding data to view model properties 
        emailTextBox.TextChanged.Add(fun _ -> viewModel.Email <- emailTextBox.Text)
        passwordBox.TextChanged.Add(fun _ -> viewModel.Password <- passwordBox.Text)
        
        loginButton.Click.Add(fun _ ->
            if viewModel.Login() then
                // successful do something here
                printf "login successful"
            else
                // shows error message
                errorTextBlock.Text <- viewModel.ErrorMessage
                )
        
        
        
    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)