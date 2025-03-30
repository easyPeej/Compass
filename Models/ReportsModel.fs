namespace Compass.Models

type [<CLIMutable>] SafeguardingReports =
    {
        id: int
        reporter_id: int
        concern_description: string
        date_reported: System.DateTime
        status: string
        assigned_staff: int64 option
        child_id: int
        severity: string
        raised_to_social_care: bool
    }
    
        

