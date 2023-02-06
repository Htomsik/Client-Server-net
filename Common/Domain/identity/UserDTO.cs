using System.ComponentModel.DataAnnotations;

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

public class UserDTO : LoginUserDTO
{
    public ICollection<RoleDTO> Roles { get; set; }
}