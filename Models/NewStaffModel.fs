namespace Compass.Models

type [<CLIMutable>] StaffModel =
    {
        first_name: string
        last_name: string
        email: string
        phone: string option
        role: string
        password_hash: string
    }
    
    
