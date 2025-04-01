namespace Compass.ViewModels

open System.Collections.ObjectModel
open Compass.Models
open Compass.Services
open ReactiveUI
open Compass.Database.Staff

type public UsersViewModel() =
    inherit ReactiveObject()
    
    let mutable viewUsers = ObservableCollection<User>()
    
    member this.AllStaffDetails
        with get() = viewUsers
        and set value =
            viewUsers <- value
            this.RaisePropertyChanged()
            
            
    member this.FetchAllStaffDetails() =
        let fetchedStaffDetails = FetchAllStaffDetails()
        this.AllStaffDetails <- ObservableCollection<User>(fetchedStaffDetails |> List.toSeq)
        
                
    