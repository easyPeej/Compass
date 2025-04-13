namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.ViewModels
open ReactiveUI

type LoginPage(onLoginSuccess: unit -> unit) as this =
    inherit UserControl()
    
    // Handles event notification 
    let LoginSuccessEvent = Event<unit>()
    
    let viewModel = LoginViewModel()
    
    
    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        
        (*let viewModel = LoginViewModel()*)
    
        let emailTextBox = this.FindControl<TextBox>("EmailTextBox")
        let passwordBox = this.FindControl<TextBox>("PasswordBox")
        let loginButton = this.FindControl<Button>("LoginButton")
        let errorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock")
        
        // binding data to view model properties 
        emailTextBox.TextChanged.Add(fun _ -> viewModel.Email <- emailTextBox.Text)
        passwordBox.TextChanged.Add(fun _ -> viewModel.Password <- passwordBox.Text)
        
        loginButton.Click.Add(fun _ ->
            if viewModel.Login() then
                (*printf "login successful"*)
                
                onLoginSuccess()
            else
                // shows error message 
                errorTextBlock.Text <- viewModel.ErrorMessage
                )
        
    member this.LoginSuccess = LoginSuccessEvent.Publish
        
    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)