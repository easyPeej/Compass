namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.Services
open Compass.ViewModels
open Compass.Views
open Avalonia.Interactivity


type MFAView(onOtpSuccess: unit -> unit) as this =
    inherit UserControl()
    
    let viewModel = MFAViewModel()

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        
        viewModel.SendOTP()
        //printfn $"MFAview.fs code: {viewModel.CurrentOtp}" // displays code for me in the terminal TESTING 
        
        let VerifyButton = this.FindControl<Button>("VerifyButton")
        let errorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock")
        
        VerifyButton.Click.Add(fun _ ->
                if viewModel.VerifyOTP() then
                    // successful OTP input goes back to MainWindow.fs to navigate to Dashboard
                    if viewModel.AttemptsOver then
                        UserSession.Logout()
                        Navigation.NavigateTo(LoginPage(ignore))
                    else
                       UserSession.OtpPassed <- true
                       onOtpSuccess()
                        
                else
                    // shows error message 
                    errorTextBlock.Text <- viewModel.ErrorMessage
                    )        
        

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
        
    member this.LogoutButton(sender: obj, e: RoutedEventArgs) =
          UserSession.Logout()
          Navigation.NavigateTo(LoginPage(ignore))
    