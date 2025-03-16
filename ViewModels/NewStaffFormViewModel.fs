namespace Compass.ViewModels

open System
open System.Collections.ObjectModel
open ReactiveUI
open Compass.Models
open Compass.Database.Staff



type NewStaffFormViewModel() =
    inherit ReactiveObject()
    

    // bind properties
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable email = ""
    let mutable password = ""
    let mutable selectedRole = ""
    let mutable phone: string option = None
    
    let roles = ObservableCollection<string>(["Admin"; "Teacher"; "Safeguarding Lead"; "Support Staff"])
    
    
    
    member this.Roles = roles
    
    member this.SelectedRole
        with get() = selectedRole
        and set value =
            selectedRole <- value
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
            
    member this.Password
        with get() = password
        and set value =
            password <- value
            this.RaisePropertyChanged()
            
    // need a confirm password member needed here
            
    member this.Email
        with get() = email
        and set value =
            email <- value
            this.RaisePropertyChanged()
            
    member this.Phone
        with get() = phone |> Option.defaultValue ""
        and set value =
            phone <- if String.IsNullOrWhiteSpace(value) then None else Some value
            this.RaisePropertyChanged()
            
    member this.CreateStaff =
        ReactiveCommand.Create(fun () ->
            let hashedPassword = BCrypt.Net.BCrypt.HashPassword(password)
            let newUser: StaffModel = {
                first_name = firstName
                last_name = lastName
                email = email
                phone = phone
                password_hash = hashedPassword
                role = selectedRole
            }
            CreateStaff newUser
            )