namespace Compass

open System.Collections
open Avalonia
open Avalonia.Controls 
open Avalonia.Markup.Xaml
open Compass.ViewModels
open Compass.Views
open Avalonia.Interactivity
open Compass.Services

type MainWindow () as this = 
    inherit Window ()
    
    let viewModel = MainWindowViewModel()
    
    let onOtpSuccess() =
        match UserSession.UserSession with
        | Some user ->
            let View = Dashboard()  
            this.NavigateToPage(View)
        | None ->
            this.NavigateToPage(this.CreateLoginPage())    
    
    // using a callback to help navigate between login and dashboard, between users
    let onLoginSuccess() =
        match UserSession.UserSession with
        | Some user ->
            let View = MFAView(onOtpSuccess)    
            this.NavigateToPage(View)
        | None ->
            this.NavigateToPage(this.CreateLoginPage())

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
            
        this.NavigateToPage(this.CreateLoginPage())    
                     
   
    // method for switching user content in the main window
    member this.NavigateToPage(page: UserControl) =
        match this.VisualRoot with
        | :? Window as mainWindow -> 
            match mainWindow.FindControl<ContentControl>("MainContent") with
            | null -> ()
            | contentControl -> 
                contentControl.Content <- page
        | _ -> ()
                
        
    // creating new members like this help us utilise the NavigateToPage method
    // FYI - 'LoginButton' is linked to the Click type in the button in main axaml, this is how they recognise
    
    member this.CreateLoginPage() =
        new LoginPage(onLoginSuccess)
    
    // not likely to need this in the end 
    member this.LoginButton(sender: obj, e: RoutedEventArgs) =
        this.NavigateToPage(this.CreateLoginPage())
          
    member this.LogoutButton(sender: obj, e: RoutedEventArgs) =
          UserSession.Logout()
          this.NavigateToPage(this.CreateLoginPage())
          
    member this.Dashboard(sender: obj, e: RoutedEventArgs) =
        let dashboard = new Dashboard()
        this.NavigateToPage(dashboard)
        
    member this.Reporting(sender: obj, e: RoutedEventArgs) =
        let reporting = new Reporting()
        this.NavigateToPage(reporting)
        
    member this.AddMember(sender: obj, e: RoutedEventArgs) =
        let addStaff = new NewStaffFormView()
        this.NavigateToPage(addStaff)
        
    member this.UpdateReport(sender: obj, e: RoutedEventArgs) =
        let updateReport = new UpdateReport(0)
        this.NavigateToPage(updateReport)
        
        
    member private this.InitializeComponent() =
#if DEBUG
        this.AttachDevTools()
#endif
        AvaloniaXamlLoader.Load(this)