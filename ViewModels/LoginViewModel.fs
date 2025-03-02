namespace Compass.ViewModels

open ReactiveUI

type LoginViewModel() =
    inherit ReactiveObject()
    
    let mutable username = ""
    let mutable password = ""
    
    
    member this.Username
        with get() = username
        and set(value) = this.RaiseAndSetIfChanged(&username, value) |> ignore
        
    member this.Password
        with get() = password
        and set(value) = this.RaiseAndSetIfChanged(&password, value) |> ignore
        
        
    member this.LoginCommand =
        ReactiveCommand.Create(fun () ->
            if this.Username = "admin" && this.Password = "password123" then
                printf "Successful"
            else
                printf "Invalid Creds")