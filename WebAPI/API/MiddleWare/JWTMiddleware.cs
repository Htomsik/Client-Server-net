using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Identity;

namespace API.MiddleWare;

internal sealed class JwtMiddleware
{
    #region Fields

    private readonly RequestDelegate _next;

    private readonly IConfigurationSection _jwtConfiguration;
    
    private readonly ILogger _logger;

    #endregion

    #region Constructors

    public JwtMiddleware(RequestDelegate next, 
        IConfiguration configuration,
        ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _jwtConfiguration = configuration.GetSection("Security:JWT");
        _logger = logger;
    }
    #endregion

    #region Methods

    public async Task Invoke(HttpContext context, UserManager<User> userManager)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if(!string.IsNullOrEmpty(token))
            AttachToContext(context, userManager, token);

        await _next(context);
    }

    private async Task AttachToContext(HttpContext context, UserManager<User> userManager, string token) 
    {
        _logger.LogInformation("Validate authorization token");
        
        var jwtSecurity = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration["Key"]));

        try
        {
            jwtSecurity.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtConfiguration["Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false
            }, out SecurityToken validToken);

            var jwtToken = (JwtSecurityToken)validToken;
        
            var userName = jwtToken.Claims.ToList().FirstOrDefault(elem => elem.Type == ClaimTypes.Name)?.Value;
            
            if (!string.IsNullOrEmpty(userName))
                context.Items["User"] = await userManager.FindByNameAsync(userName).ConfigureAwait(false);
            
            _logger.LogInformation("Validate authorization token success. UserName: {userName}",userName);
        }
        catch (Exception error)
        {
            _logger.LogInformation("Validate authorization token failed");
            _logger.LogError(error, "{Source}:{Message}", error.Source, error.Message);
        }
    }
    #endregion
}