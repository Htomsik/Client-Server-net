using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Identity;
using Services.Identity;

namespace API.Services;

public class AuthService : IAuthService
{
    #region Fileds

    private readonly SymmetricSecurityKey _key;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;
    
    private readonly IMapper _mapper;

    #endregion

    #region Constructors

    public AuthService(
        IConfiguration configuration, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager,
        IMapper mapper)
    {
    
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:TokenKey"]));
    }

    #endregion
    
    #region Token creation

    private async Task<string> CreateToken(LoginUserDTO user)
    {
        var claims = new List<Claim> { new (JwtRegisteredClaimNames.NameId, user.Name) };
        
        var token = GenerateTokenOptions(claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task<string?> CreateRefreshToken(LoginUserDTO loginUser)
    {
        var user = await _userManager.FindByNameAsync(loginUser.Name);
        
        await _userManager.RemoveAuthenticationTokenAsync(user, "API", "RefreshToken");
        
        var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "API", "RefreshToken");
        
        var result = await _userManager.SetAuthenticationTokenAsync(user, "API", "RefreshToken", newRefreshToken);

        if (!result.Succeeded)
            return null;
        
        return newRefreshToken;
    }

    private JwtSecurityToken GenerateTokenOptions(List<Claim> authClaims)
    {
        
        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    #endregion

    #region IAuthService

    public async Task<Tokens?> Authorize(LoginUserDTO loginUser)
    {
        var loginResult = await _signInManager.PasswordSignInAsync(
            loginUser.Name,
            loginUser.Password,
            true,
            false);
        
        if (!loginResult.Succeeded)
            return null;

        return new Tokens
        {
            Token = await CreateToken(loginUser),
            RefreshToken = await CreateRefreshToken(loginUser),
        };
    }

    public async Task<Tokens?> Registration(LoginUserDTO loginUser)
    {
        var user = _mapper.Map<User>(loginUser);

        var registerResult = await _userManager.CreateAsync(user, loginUser.Password);

        if (!registerResult.Succeeded)
            return null;
        
        await _userManager.AddToRoleAsync(user, Role.Users);

        return await Authorize(loginUser);
    }

    public async Task<Tokens?> RefreshTokens(Tokens tokens) => new ();
    
    #endregion

}