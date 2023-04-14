using System.ComponentModel.DataAnnotations;
using Interfaces.Entities;
using Interfaces.Other;

namespace Domain.identity;

public class LoginUserDTO 
{
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
}

public class UserDTO : LoginUserDTO, IUser
{
    public int Id { get; set; }

    public ITokens Tokens { get; set; }
    
    public ICollection<RoleDTO> Roles { get; set; }
}