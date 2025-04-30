namespace Compass.Services


open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Compass.Models
open System
open System.Timers
open Compass.Services.Navigation


type UserSession private () as this =
    static let mutable userSession: User option = None
    static let mutable LoggedIn = false
    static let sessionChanged = Event<unit>()
    static let mutable otpPassed = false
    
    // for session timeout
    static let mutable lastActive = DateTime.UtcNow
    static let mutable sessionTimeout = 10.0 // session timeout 10 minutes
    //static let mutable sessionTimeout = 0.2 // session timeout 12 seconds - TESTING
    static let mutable timerCheck = new Timer(60_000.0) // checking every 60 seconds
    //static let mutable timerCheck = new Timer(2_000.0) // checking every 2 seconds - TESTING
    
    static let sessionExpired = Event<unit>()
    
    static member UserSessionChanged = sessionChanged.Publish
    static member LoggedInCheck
        with get() = LoggedIn
        
    static member OtpPassedCheck
        with get() = otpPassed
    
    static member UserSession
        with get() = userSession
        and set(value) =
            userSession <- value
            LoggedIn <- value.IsSome
            lastActive <- DateTime.UtcNow // time of active login
            //printfn $"Last active time updated: {lastActive}" // TESTING
            sessionChanged.Trigger()
            
    static member SessionExpired = sessionExpired.Publish
            
    static member GetUserId() =
        match userSession with
        | Some user -> Some user.id
        | None -> None
        
    static member GetRole() =
        match userSession with
        | Some user -> Some user.role
        | None -> None

    static member Logout() =
        //printfn "Session timed out. Logging out..." // FOR TESTING
        userSession <- None
        LoggedIn <- false
        otpPassed <- false
        sessionChanged.Trigger()
        
    static member OtpPassed
        with get() = otpPassed
        and set value =
            otpPassed <- value
            sessionChanged.Trigger()
    
    static member RaiseSessionChanged() =
        sessionChanged.Trigger()
        
    static member RefreshSession() =
        //printfn "refreshed timer" // TESTING
        lastActive <- DateTime.UtcNow
        
    static member StartSessionTimer() =
        timerCheck.Elapsed.Add(fun _ ->
            if LoggedIn then
                //printfn "elapsed is running" // TESTING
                let elaspedTime = DateTime.UtcNow - lastActive
                if elaspedTime.TotalMinutes >= sessionTimeout then
                    UserSession.Logout()
                    sessionExpired.Trigger()
        )
        timerCheck.AutoReset <- true
        timerCheck.Start()
