namespace Compass.ViewModels

open Compass.Models
open Compass.Services
open ReactiveUI

type public DashboardViewModel() =
    inherit ReactiveObject()
    
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable role = ""
    
    do
        match UserSession.UserSession with
        | Some user ->
            firstName <- user.first_name
            lastName <- user.last_name
            role <- user.role
        | None ->
            firstName <- ""
            lastName <- ""
            role <- ""
            
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
            
    member this.FullName =
        $"{firstName} {lastName}"

            