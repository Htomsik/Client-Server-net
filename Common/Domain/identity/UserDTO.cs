using System.ComponentModel.DataAnnotations;
using Interfaces.Entities;
using Interfaces.Other;

namespace Domain.identity;

public class LoginUserDTO  : IAuthUser
{
    public int Id { get; set; }
    
    [Required]
    [MinLength(3)]
    [MaxLength(10)]
    public string Name { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [MaxLength(16)]
    public string Password { get; set; }
}

public class RegistratonUserDTO : LoginUserDTO, IRegUser
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}

public class UserDTO : RegistratonUserDTO, IUser
{
    public ITokens Tokens { get; set; }
    
    public ICollection<RoleDTO> Roles { get; set; }
}