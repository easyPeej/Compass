namespace Compass.ViewModels

open System
open BCrypt.Net
open Compass.Database.Staff
open Dapper
open ReactiveUI
open Compass.Services
open Compass.Models
open Compass.Database



type LoginViewModel() =
    inherit ReactiveObject()
    
    
    
    // bind properties
    let mutable email = ""
    let mutable password = ""
    let mutable errorMessage = ""
    let mutable lastLoginTime = DateTime
    
    member this.Email
        with get() = email
        and set value =
            email <- value
            this.RaisePropertyChanged()
            
    member this.Password
        with get() = password
        and set value =
            password <- value
            this.RaisePropertyChanged()
            
    member this.ErrorMessage
        with get() = errorMessage
        and set value =
            errorMessage <- value
            this.RaisePropertyChanged()
            
    member this.LastLoginTime
        with get() = lastLoginTime
        and set value =
            lastLoginTime <- value
            this.RaisePropertyChanged()
    
    
   // Test method to check the session singleton works as intended and stores user data on login        
    member this.CheckSession() =
        match UserSession.UserSession with
        | Some user ->
            printf $"Logged in as: %s{user.first_name} %s{user.last_name} as %s{user.role} (Email: %s{user.email})"
            this.ErrorMessage <- $"Welcome, {user.first_name}"
        | None ->
            printf "no user logged in"
            this.ErrorMessage <- "No active session"
    // Test method -----------------------------------------------------------------------------
   
        
    member this.Login() =
        if String.IsNullOrWhiteSpace(this.Email) || String.IsNullOrWhiteSpace(this.Password) then
            this.ErrorMessage <- "Email and Password are required"
            false
        else
            // login function in staff.fs
            let result = Login(this.Email)
                               
            match result with
            | None ->
                this.ErrorMessage <- "Invalid email or password." 
                false
            | Some user ->
                if BCrypt.Verify(this.Password.Trim(), user.password_hash.Trim()) then
                    UserSession.UserSession <- Some user
                    this.ErrorMessage <- ""
                    // Using the session tester method
                    this.CheckSession()
                    true
                else
                    this.ErrorMessage <- "Invalid email or password."
                    false
                    