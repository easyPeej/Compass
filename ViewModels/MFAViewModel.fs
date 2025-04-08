namespace Compass.ViewModels

open ReactiveUI

type MFAViewModel(expectedCode: string) as this =
    inherit ReactiveObject()
    
    let mutable inputCode =""
    
    member this.InputCode
        with get() = inputCode
        and set value =
            inputCode <- value
            this.RaisePropertyChanged()
            
    (*member this.VerifyCode =
        ReactiveCommand.Create(fun () ->
            if this.InputCode = expectedCode then
                true
            else
                false
                )*)
        