namespace Compass.Services

open System
open Compass.Models

type UserSession private () =
    static let mutable userSession: User option = None
    
    static member UserSession
        with get() = userSession
        and set(value) = userSession <- value
        
        static member LoggedIn = userSession.IsSome

    