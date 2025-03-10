namespace Compass.ViewModels

open Compass.Database.Reports
open ReactiveUI

type ReportsViewModel() =
    inherit ReactiveObject()
    
    let mutable fullReport = []
    let mutable count = 0
    
    member public this.FullReport
        with get() = fullReport
        and set value =
            fullReport <- value
            this.RaisePropertyChanged()
        
    member this.LoadAllReports() =
        let fetchedReports = FetchReports()
        this.FullReport <- fetchedReports
        
        
    
        
        
    
    
    
    