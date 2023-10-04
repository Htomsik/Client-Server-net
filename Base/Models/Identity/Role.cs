using Microsoft.AspNetCore.Identity;

namespace Models.Identity;

public class Role : IdentityRole<int>
{
    public const string Administrators = "Administrators";

    public const string Users = "Users";
}