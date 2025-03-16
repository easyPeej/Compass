namespace Compass

open Avalonia
open Avalonia.Controls 
open Avalonia.Markup.Xaml
open Compass.Views
open Avalonia.Interactivity
open Compass.Services

type MainWindow () as this = 
    inherit Window ()

    do
        this.InitializeComponent()
            
    do
        let loginPage = new LoginPage()
        this.FindControl<ContentControl>("MainContent").Content <- loginPage
        
        loginPage.LoginSuccess.Add(fun _ ->
            let dashboard = new Dashboard()
            this.FindControl<ContentControl>("MainContent").Content <- dashboard)
    
        
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
    member this.LoginButton(sender: obj, e: RoutedEventArgs) =
          let loginPage = new LoginPage()
          this.NavigateToPage(loginPage)
          
    member this.LogoutButton(sender: obj, e: RoutedEventArgs) =
          UserSession.Logout()
          let loginPage = new LoginPage()
          this.NavigateToPage(loginPage)
          
    member this.Dashboard(sender: obj, e: RoutedEventArgs) =
        let dashboard = new Dashboard()
        this.NavigateToPage(dashboard)
        
    member this.Reporting(sender: obj, e: RoutedEventArgs) =
        let reporting = new Reporting()
        this.NavigateToPage(reporting)
        
    member this.AddMember(sender: obj, e: RoutedEventArgs) =
        let addStaff = new NewStaffFormView()
        this.NavigateToPage(addStaff)        
        
        
    member private this.InitializeComponent() =
#if DEBUG
        this.AttachDevTools()
#endif
        AvaloniaXamlLoader.Load(this)