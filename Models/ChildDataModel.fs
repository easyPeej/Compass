namespace Compass.Models

type [<CLIMutable>] ChildDataModel =
    {
        Id: int
        first_name: string
        last_name: string
        date_of_birth: System.DateTime
        gender: string
        address: string
        school: string option  
        emergency_contact: string
        social_worker: string option  
        risk_level: string
        medical_notes: string option  
        safeguarding_notes: string option  
    }


