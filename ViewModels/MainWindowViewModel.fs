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
    let mutable teacherNav = false
    let mutable adminNav = false
    let mutable leadStaffNav = false
    let mutable supportNav = false
    

    do
        UserSession.UserSessionChanged.Add(fun _ ->
            this.isLoggedInCheck <- UserSession.LoggedInCheck
            this.IsNavBarEnabled <- UserSession.LoggedInCheck && UserSession.OtpPassedCheck
            match UserSession.UserSession with
            | Some user ->
                this.Role <- user.role
            | None ->
                this.Role <- ""
            
            
            match this.Role with
            | "Admin" ->
                this.IsAdminNav <- true
            | "Teacher" ->
                this.IsTeacherNav <- true
            | "Safeguarding Lead" ->
                this.IsLeadNav <- true
            | "Support Staff" ->
                this.IsSupportNav <- true
            | _ ->
                this.IsAdminNav <- false
                this.IsLeadNav <- false
                this.IsTeacherNav <- false
                this.IsSupportNav <- false)
        
            
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
    member this.IsTeacherNav
        with get() = teacherNav
        and set value =
            teacherNav <- value
            this.RaisePropertyChanged()        
    member this.IsAdminNav
        with get() = adminNav
        and set value =
            adminNav <- value
            this.RaisePropertyChanged()
            
    member this.IsLeadNav
        with get() = leadStaffNav
        and set value =
            leadStaffNav <- value
            this.RaisePropertyChanged()            
    
    member this.IsSupportNav
        with get() = supportNav
        and set value =
            supportNav <- value
            this.RaisePropertyChanged()
            
    member this.Role
        with get() = role
        and set value =
            role <- value
            this.RaisePropertyChanged()
            