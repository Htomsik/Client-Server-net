using AutoMapper;
using Domain.Entities;
using Domain.identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Identity;
using Services.Identity;

namespace API.Controllers.Identity;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    #region Fileds

    private readonly ILogger<AccountController> _logger;
    
    private readonly IAuthService _authService;
    
    private readonly IMapper _mapper;

    #endregion

    #region Constructors

    public AccountController(ILogger<AccountController> logger, IAuthService authService, IMapper mapper)
    {
        _logger = logger;
        _authService = authService;
        _mapper = mapper;
    }
    
    #endregion
    
    #region Controllers

    #region Login
    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Login(LoginUserDTO userDto)
    {
        _logger.LogInformation("Login attempt: {0}",userDto.Name);

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("User {0} model invalid", userDto.Name);
            return BadRequest(null!);
        }
        
        var tokens = await _authService.Authorize(userDto);

        if (tokens is null)
        {
            _logger.LogInformation("User {0} authorize denied", userDto.Name);
            return Unauthorized(null!);
        }
        
        _logger.LogInformation("User {0} authorize successful", userDto.Name);
        
        return Accepted(_mapper.Map<TokensDTO>(tokens));
    }

    #endregion
    
    #region Registration
    [AllowAnonymous]
    [HttpPost("Registration")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Registration(LoginUserDTO userDto)
    {
        _logger.LogInformation("Registration attempt: {0}",userDto.Name);

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("User {0} model invalid", userDto.Name);
            return BadRequest(null!);
        }
        
        var tokens = await _authService.Registration(userDto);

        if (tokens is null)
        {
            _logger.LogInformation("User {0} registration denied", userDto.Name);
            return Unauthorized(null!);
        }
        
        _logger.LogInformation("User {0} registration successful", userDto.Name);
        
        return Accepted(_mapper.Map<TokensDTO>(tokens));
    }

    #endregion

    #region RefreshTokens
    [AllowAnonymous]
    [HttpPost("RefreshTokens")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> RefreshTokens(TokensDTO tokensDto)
    {
        _logger.LogInformation("Refresh token attempts");

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Token model invalid");
            return BadRequest(null!);
        }

        var tokens = await _authService.RefreshTokens(_mapper.Map<Tokens>(tokensDto));

        if (tokens is null)
        {
            _logger.LogInformation("Refresh token denied");
            return Unauthorized(null!);
        }
        
        _logger.LogInformation("Refresh token successful");

        return Accepted(_mapper.Map<TokensDTO>(tokens));
    }

    #endregion

    #endregion
}