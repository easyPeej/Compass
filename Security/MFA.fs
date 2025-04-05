namespace Compass.Security

open System.Net.Http
open QRCoder
open System.IO
open Avalonia.Controls
open Avalonia.Markup.Xaml
open Avalonia.Media.Imaging

module MFA = 
    
    (*let GenKey() =
        let key = KeyGeneration.GenerateRandomKey(20)
        Base32Encoding.ToString(key)
        
    let genQrCode(userEmail: string) (secretKey: String) =
        let issuer = "Compass"
        let otpUri = $"otpauth://totp/{issuer}:{userEmail}?secret={secretKey}&issuer={issuer}&digits=6"
        
        let qrGen = new QRCodeGenerator()
        let qrData = qrGen.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q)
        let qrCode = new 
        let qrImage = qrCode.GetGraphic(20)
        
        let path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MFAQRCode.png")
        qrImage.Save(path, ImageFormat.Png)*)
    
    
        
    // generate a qr code and convert to png     
    let GenQrCodeBytes(content: string) =
        let qrGen = new QRCodeGenerator()
        let qrData = qrGen.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q)
        let qrCode = new BitmapByteQRCode(qrData)
        let byteArray = qrCode.GetGraphic(20)
        byteArray
        
    let ToBitmap (byteArray: byte[]) =
        let stream = new MemoryStream(byteArray)
        new Bitmap(stream)
        
    let DisplayQrCode(content: string) =
        let qr = GenQrCodeBytes(content)
        
        let qrBitmap = ToBitmap(qr)
        
        match this.FindControl<Image>("QrCodeImage") with
        | null -> ()
        | imageConrol -> imageConrol.Source <- qrBitmap