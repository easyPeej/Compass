namespace Compass.Database

open Compass.Database.DbConnect
open Dapper
open Microsoft.Data.Sqlite
open Compass.Models


module Staff =
    
    let CreateStaff (staff: StaffModel) =
        use connection = GetConnection()
        
        let query = "
            INSERT INTO Users (first_name, last_name, email, password_hash, role, phone, created_at, last_login, status, permissions)
            VALUES (@FirstName, @LastName, @Email, @Password, @Role, @Phone, @CreatedAt, @LastLogin, @Status, @Permissions)"
        
        let parameters =
            {| FirstName = staff.first_name
               LastName = staff.last_name
               Email = staff.email
               Password = staff.password_hash
               Role = staff.role
               Phone = staff.phone |> Option.defaultValue null
               CreatedAt = System.DateTime.Now
               LastLogin = null
               Status = "Active"
               Permissions = "Standard" |}
              
        connection.Execute(query, parameters) |> ignore
        
    let FetchAllStaffDetails() =
        use connection = DbConnect.GetConnection()
        let query = "SELECT * FROM Users"
        connection.Query<User>(query)
        |> Seq.toList
        
    let FetchStaff() =
        use connection = DbConnect.GetConnection()
        let query = "SELECT first_name || ' ' || last_name FROM Users"
        connection.Query<string>(query)
        |> Seq.toList
        
        
    let LastLoginUpdate(email) =
        use connection = DbConnect.GetConnection()
        let query = "UPDATE Users SET last_login = @DateTime WHERE email = @Email"
        connection.Query<User>(query, {| Email = email; DateTime = System.DateTime.Now |})
        
    let Login(email) =
        use connection = DbConnect.GetConnection()
        
        let query = "SELECT * FROM Users WHERE email = @Email"
        let result =
            connection.Query<User>(query, {| Email = email |})
            |> Seq.tryHead
        LastLoginUpdate(email) |> ignore
        result
        
