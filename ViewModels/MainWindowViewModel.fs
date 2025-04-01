namespace Compass.ViewModels

open Avalonia.Controls
open Compass.Models
open Compass.Services
open ReactiveUI


type MainWindowViewModel() as this =
    inherit ReactiveObject()
    
    let mutable role = ""
    let mutable isLoggedInCk = UserSession.LoggedInCheck
    let mutable isNavBarEnabled = false

    do
        UserSession.UserSessionChanged.Add(fun _ ->
            this.isLoggedInCheck <- UserSession.LoggedInCheck
            this.IsNavBarEnabled <- UserSession.LoggedInCheck)
        
        match UserSession.UserSession with
        | Some user ->
            role <- user.role
        | None ->
            role <- ""
    
    member this.isLoggedInCheck
        with get() = isLoggedInCk
        and set value =
        isLoggedInCk <- value
        this.RaisePropertyChanged()
        
    member this.IsNavBarEnabled
        with get() = isNavBarEnabled
        and set value =
            isNavBarEnabled <- value
            this.RaisePropertyChanged()
            
    member this.Role
        with get() = role
        and set value =
            role <- value
            this.RaisePropertyChanged()
            