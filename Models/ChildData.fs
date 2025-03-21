namespace Compass.Models

type [<CLIMutable>] ChildData =
    {
        Id: int
        FirstName: string
        LastName: string
        DateOfBirth: System.DateTime
        Gender: string
        Address: string
        School: string option  
        EmergencyContact: string
        SocialWorker: string option  
        RiskLevel: string
        MedicalNotes: string option  
        SafeguardingNotes: string option  
    }


