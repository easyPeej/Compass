namespace Compass.ViewModels

open System
open ReactiveUI
open Compass.Services
open Compass.Security.OTP


type MFAViewModel() as this =
    inherit ReactiveObject()
    
    let mutable inputCode =""
    let mutable email = ""
    let mutable latestCode = ""
    let mutable errorMessage = ""
    let mutable attemptsCount = 3
    let mutable attemptsOver = false
    
    do
        match UserSession.UserSession with
        | Some user ->
            email <- user.email
        | None ->
            email <- ""
            
        // TESTING    
        //printfn $"MFA View Model test email: {this.Email}"       

    member this.Email
        with get() = email
        and set value =
            email <- value
            this.RaisePropertyChanged()
    
    member this.InputCode
        with get() = inputCode
        and set value =
            inputCode <- value
            this.RaisePropertyChanged()
           
    member this.ErrorMessage
        with get() = errorMessage
        and set value =
            errorMessage <- value
            this.RaisePropertyChanged()            
           
    member this.CurrentOtp
        with get() = latestCode
        and set value =
            latestCode <- value
            this.RaisePropertyChanged()
            
    member this.OtpAttemptCount
        with get() = attemptsCount
        and set value =
            attemptsCount <- value
            this.RaisePropertyChanged()
            
    member this.AttemptsOver 
        with get() = attemptsOver
        and set value =
            attemptsOver <- value
            this.RaisePropertyChanged()
            
    member this.SendOTP () =
        let code = GenerateCode()
        this.CurrentOtp <- code
        
        (* Disable this for testing *)
        (*do SendEmail this.Email code |> ignore *) 
    
    member this.VerifyOTP() =
        let code = this.InputCode.Trim()
        
        if String.IsNullOrWhiteSpace(code) then
            this.ErrorMessage <- "You must input your OTP sent to your email address"
            false
            
        elif code.Length <> 6 then
            this.ErrorMessage <- "The OTP must be 6 numbers in length"
            false
            
        elif code = this.CurrentOtp then
                this.OtpAttemptCount <- 3
                this.CurrentOtp <- ""
                this.AttemptsOver <- false
                UserSession.OtpPassed <- true
                true

                
        else // incorrect attempts 
            this.OtpAttemptCount <- this.OtpAttemptCount - 1
            
            if this.OtpAttemptCount <= 0 then
                this.AttemptsOver <- true
                true
            else
                this.ErrorMessage <- $"Incorrect OTP Code, Attempts remaining: {this.OtpAttemptCount}"
                false
            
            

                    