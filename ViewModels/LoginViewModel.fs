namespace Compass.ViewModels

open System
open BCrypt.Net
open Dapper
open Microsoft.Data.Sqlite
open Avalonia.Controls
open Avalonia.Interactivity
open ReactiveUI
open Compass.Models



type LoginViewModel() =
    inherit ReactiveObject()
    
    // bind properties
    let mutable email = ""
    let mutable password = ""
    let mutable errorMessage = ""
    
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
            
    member this.Login() =
        if String.IsNullOrWhiteSpace(this.Email) || String.IsNullOrWhiteSpace(this.Password) then
            this.ErrorMessage <- "Email and Password are required"
            false
        else
            use connection = new SqliteConnection("Data Source=Data.db")
            connection.Open()
            
            let sql = "SELECT * FROM Users WHERE email = @Email"
            let result =
                connection.Query<User>(sql, {| Email = this.Email |})
                |> Seq.tryHead
            
            (*match userOption with
            | Null ->
                this.ErrorMessage <- "HI"
                false
            | user ->
                if BCrypt.Verify(this.Password, user.password_hash) then
                    this.ErrorMessage <- ""
                    true
                else
                    this.ErrorMessage <- "Invalid email or password"
                    false*)
                    
            match result with
            | None ->
                this.ErrorMessage <- "Invalid email or password"
                false
            | Some user ->
                if BCrypt.Verify(this.Password, user.password_hash) then
                    this.ErrorMessage <- ""
                    true
                else
                    this.ErrorMessage <- "Invalid email or password"
                    false


