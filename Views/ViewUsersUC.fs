namespace Compass.Views 

open Avalonia.Controls
open Avalonia.Markup.Xaml
open Compass.Database
open Compass.Database.Staff
open Compass.ViewModels


type ViewUsersUC() as this =
    inherit UserControl()
    
    let viewModel = UsersViewModel()

    do
        this.InitializeComponent()
        this.DataContext <- viewModel
        viewModel.FetchAllStaffDetails()

    member private this.InitializeComponent() =
        AvaloniaXamlLoader.Load(this)
        printfn $"{FetchAllStaffDetails}"
