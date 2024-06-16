using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace StudentManager;

public class Util
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
    
    public static Expression<Func<Student,object>> GetSortingPropertySelectorLambda(string field)
    {
        return (field.ToLower()) switch
        {
            "firstname" => s => s.FirstName,
            "lastname" => s => s.LastName,
            "email" => s => s.Email,
            "nic" => s => s.NIC,
            "mobile" => s => s.Mobile,
            "dob" => s => s.DateOfBirth,
            "address" => s => s.Address,
            "createdat" => s => s.CreatedAt,
            _ => s => s.Id,
        };

    }
}