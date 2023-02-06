using Microsoft.AspNetCore.Identity;

namespace Models.Identity;

public class User : IdentityUser<int>
{
    public const string Administrator = "Admin";

    public const string DefaultAdminPassword = "Admin_Pass";
    
}