namespace Compass.Services


open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Compass.Models


type UserSession private () =
    static let mutable userSession: User option = None
    
    // using LoggedIn to detect whether user is logged in or not for certain UI elements
    // THIS NEEDS SOME SERIOUS WORK FGS
    static let mutable LoggedIn = false
    static let sessionChanged = Event<unit>()
    
    static member UserSessionChanged = sessionChanged.Publish
    static member LoggedInCheck
        with get() = LoggedIn
    
    static member UserSession
        with get() = userSession
        and set(value) =
            userSession <- value
            LoggedIn <- value.IsSome
            sessionChanged.Trigger()
            
    static member GetUserId() =
        match userSession with
        | Some user -> Some user.id
        | None -> None

    
    static member Logout() =
        userSession <- None
        LoggedIn <- false
        sessionChanged.Trigger()
        
        
        
        