namespace Compass.Security

open System
open SendGrid.Helpers.Mail
open Twilio
open Twilio.Rest.Api.V2010.Account
open Twilio.Types
open SendGrid
open SendGrid.Helpers
open System.Threading.Tasks

module OTP =

    let accountSid = "VA231721bee20f09b6b47b33e190eab03b"
    let authToken = "[41c9e2d80dda183168f276e5e71e5030]"
    
    let GenerateCode() =
        let rnd = System.Random()
        rnd.Next(100000, 999999).ToString()
    
    let SendEmail (toEmail: string) (code: string) =
        let apiKey = "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"
        let client = new SendGridClient(apiKey)
        let from = EmailAddress("pscottcaven@gmail.com", "Compass")
        let subject = "Your OTP Verification Code"
        let toAddress = EmailAddress(toEmail)
        let plaintextContent = $"Your OTP Verification Code: {code}"
        let htmlContent = $"<strong>Your MFA code is: {code}</strong>"
        let msg = MailHelper.CreateSingleEmail(from, toAddress, subject, plaintextContent, htmlContent)
        let response = client.SendEmailAsync(msg)
        printfn $"Sending OTP email to {toEmail}"
        response.Status