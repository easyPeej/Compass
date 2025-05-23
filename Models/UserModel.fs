﻿namespace Compass.Models

type [<CLIMutable>] User =
    {
        id: int
        first_name: string
        last_name: string
        email: string
        password_hash: string
        role: string
        phone: string option
        created_at: System.DateTime
        last_login: string option
        status: string
        permissions: string
    }


