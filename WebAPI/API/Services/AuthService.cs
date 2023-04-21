using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Infrastructure.Extensions;
using AutoMapper;
using Domain.identity;
using Interfaces.Other;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Identity;
using Services.Identity;

namespace API.Services;

public class AuthService : IAuthService<LoginUserDTO,RegistratonUserDTO,UserDTO, Tokens>
{
    #region Fileds
    
    private readonly IConfigurationSection _jwtConfiguration;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;
    
    private readonly IMapper _mapper;

    #endregion
    
    #region Constants
    private const string RefreshTokenName = "RefreshToken";
    #endregion

    #region Constructors

    public AuthService(
        IConfiguration configuration, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager,
        IMapper mapper)
    {
        _jwtConfiguration = configuration.GetSection("Security:JWT");
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    #endregion
    
    #region Token creation

    private async Task<string> CreateToken(LoginUserDTO user)
    {
        var claims = new List<Claim> { new (ClaimTypes.Name, user.Name) };
        
        var token = GenerateTokenOptions(claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task<string?> CreateRefreshToken(LoginUserDTO loginUser)
    {
        var user = await _userManager.FindByNameAsync(loginUser.Name);
        
        await _userManager.RemoveAuthenticationTokenAsync(user, "API", RefreshTokenName);
        
        var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, _jwtConfiguration["Issuer"]!, RefreshTokenName);
        
        var result = await _userManager.SetAuthenticationTokenAsync(user, _jwtConfiguration["Issuer"]!, RefreshTokenName, newRefreshToken);

        if (!result.Succeeded)
            return null;
        
        return newRefreshToken;
    }

    private JwtSecurityToken GenerateTokenOptions(List<Claim> authClaims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration["Key"]));
        
        var expiration = DateTime.Now.AddHours(Convert.ToDouble(
            _jwtConfiguration.GetSection("LifetimeHours").Value));

        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.GetSection("Issuer").Value,
            expires: expiration,
            claims: authClaims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    #endregion

    #region IAuthService

    public async Task<Tokens?> Authorize(LoginUserDTO loginUser, CancellationToken cancel = default)
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

    public async Task<Tokens?> Registration(RegistratonUserDTO loginUser, CancellationToken cancel = default)
    {
        var user = _mapper.Map<User>(loginUser);

        var registerResult = await _userManager.CreateAsync(user, loginUser.Password);

        if (!registerResult.Succeeded)
            return null;
        
        await _userManager.AddToRoleAsync(user, Role.Users);

        return await Authorize(loginUser, cancel);
    }

    public async Task<bool> Deactivate(Tokens tokens, CancellationToken cancel = default)
    {
        if (!tokens.Validate(_jwtConfiguration))
            return false;
        
        var user = await GetUserFromToken(tokens);

        if (user is null)
            return false;

        
        var result = await _userManager.SetLockoutEnabledAsync(user, true);

        if (result.Succeeded)
            result =  await _userManager.SetLockoutEndDateAsync(user, DateTime.Today.AddYears(100));
            
        return result.Succeeded;
    }
    
    public async Task<Tokens?> RefreshTokens(Tokens tokens, CancellationToken cancel = default)
    {
        var user = await GetUserFromToken(tokens);

        if (user is null)
            return null;
        
        var isValid = await _userManager.VerifyUserTokenAsync(user, _jwtConfiguration.GetSection("Issuer").Value, 
            RefreshTokenName, tokens.RefreshToken);

        if (isValid)
        {
            var loginUser = _mapper.Map<LoginUserDTO>(user);
            
            return new Tokens
            {
                Token = await CreateToken(loginUser),
                RefreshToken = await CreateRefreshToken(loginUser),
            };
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return null;
    }

    public async Task<UserDTO?> Info(Tokens tokens, CancellationToken cancel = default)
    {
        if (!tokens.Validate(_jwtConfiguration))
            return null;
        
        var user = await GetUserFromToken(tokens);

        if (user is null)
            return null;
        
        var userDto = _mapper.Map<UserDTO>(user);
        
        userDto.Roles = await _userManager.GetRolesAsync(user);

        return userDto;
    }

    private async Task<User?> GetUserFromToken(ITokens tokens)
    {
        var userName = tokens.UserName();
        
        if (string.IsNullOrEmpty(userName))
            return null;

        return await _userManager.FindByNameAsync(userName);
    }
    
    #endregion
}