namespace Compass.ViewModels

open Compass.Models
open Compass.Services
open ReactiveUI

type public DashboardViewModel() =
    inherit ReactiveObject()
    
    // User session information - mostly for display 
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable role = ""
    let mutable id: int64 = 0
    
    
    do
        match UserSession.UserSession with
        | Some user ->
            firstName <- user.first_name
            lastName <- user.last_name
            role <- user.role
            id <- user.id
        | None ->
            firstName <- ""
            lastName <- ""
            role <- ""
            id <- 0
            
    member this.Id
        with get() = id
        and set value =
            id <- value
            this.RaisePropertyChanged()
            
    member this.FirstName
        with get() = firstName
        and set value =
            firstName <- value
            this.RaisePropertyChanged()
            
    member this.LastName
        with get() = lastName
        and set value =
            lastName <- value
            this.RaisePropertyChanged()
            
    member this.Role
        with get() = role
        and set value =
            role <- value
            this.RaisePropertyChanged()
            
    // Banner display 
    member this.FullName =
        $"{firstName} {lastName}"
        

            