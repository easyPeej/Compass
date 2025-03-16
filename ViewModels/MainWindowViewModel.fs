namespace Compass.ViewModels

open Compass.Models
open Compass.Services
open ReactiveUI

type MainWindowViewModel() as this =
    inherit ReactiveObject()
    
    let mutable isLoggedInCk = UserSession.LoggedInCheck

    do
        UserSession.UserSessionChanged.Add(fun _ ->
            this.isLoggedInCheck <- UserSession.LoggedInCheck)
        
    member this.isLoggedInCheck
        with get() = isLoggedInCk
        and set value =
        isLoggedInCk <- value
        this.RaisePropertyChanged()