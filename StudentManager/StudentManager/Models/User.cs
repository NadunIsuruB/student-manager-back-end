using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace StudentManager;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}