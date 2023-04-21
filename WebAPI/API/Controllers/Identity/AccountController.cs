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
    
    private readonly IAuthService<LoginUserDTO,RegistratonUserDTO,UserDTO, Tokens> _authService;
    
    private readonly IMapper _mapper;

    #endregion

    #region Constructors

    public AccountController(ILogger<AccountController> logger, IAuthService<LoginUserDTO,RegistratonUserDTO,UserDTO, Tokens> authService, IMapper mapper)
    {
        _logger = logger;
        _authService = authService;
        _mapper = mapper;
    }
    
    #endregion
    
    #region Controllers

    #region Authorization
    [AllowAnonymous]
    [HttpPost("Authorization")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Authorization(LoginUserDTO userDto)
    {
        _logger.LogInformation("Authorization attempt: {0}",userDto.Name);

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
    public async Task<IActionResult> Registration(RegistratonUserDTO userDto)
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
    
    #region Deactivate
    [Authorize]
    [HttpDelete("Deactivate")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Deactivate(TokensDTO tokensDto)
    {
        _logger.LogInformation("Deactivate Account attempts");

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Token model invalid");
            return BadRequest(null!);
        }

        var tokens = await _authService.Deactivate(_mapper.Map<Tokens>(tokensDto));

        if (tokens == false)
        {
            _logger.LogInformation("Deactivate Account denied");
            return NotFound(null!);
        }
        
        _logger.LogInformation("Deactivate Account successful");

        return Ok(true);
    }
    
    #endregion

    #region Info

    [AllowAnonymous]
    [HttpPost("Info")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Info(TokensDTO tokensDto)
    {
        _logger.LogInformation("Info attempt");

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("Token model invalid");
            return BadRequest(null!);
        }

        var user = await _authService.Info(_mapper.Map<Tokens>(tokensDto));

        if (user is null)
        {
            _logger.LogInformation("Info denied");
            return NotFound(null!);
        }
        
        _logger.LogInformation("User {0} Info successful", user.Name);
        
        return Ok(user);
    }

    #endregion

    #endregion
}