using System.Net;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.Other;
using Core.Infrastructure.Stores.Interfaces;
using Interfaces.Entities;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Services.Identity;

namespace Core.Infrastructure.Services.AccountService;

public sealed class TokenService : ReactiveObject, ITokenService
{
    #region Fields

    private readonly IAuthService<AuthUser, RegUser, Tokens> _authService;
    
    private readonly ILogger<TokenService> _logger;
    
    private readonly IUiThreadOperation _uiThreadOperation;

    private IUser _account;

    private ISaverStore<User, bool> _userStore;

    #endregion

    #region Constructors

    public TokenService(
        IAuthService<AuthUser, RegUser, Tokens> authService, 
        ILogger<TokenService> logger,
        IUiThreadOperation uiThreadOperation,
        ISaverStore<User, bool> userStore)
    {
        _authService = authService;
        
        _logger = logger;
        
        _uiThreadOperation = uiThreadOperation;
        
        _userStore = userStore;
        
        _account = userStore.CurrentValue;

        userStore.CurrentValueChangedNotifier += _ => _account = userStore.CurrentValue;
    }

    #endregion
    
    public async Task<bool> Refresh(CancellationToken cancel = default)
    {
        Tokens? tokens = null;

        bool ret = true;
        
        _logger.LogInformation("Refresh tokens attempt. Account: {acc}", _account.Name);
        
        try
        {
            tokens = await _authService.RefreshTokens((Tokens)_account.Tokens, cancel).ConfigureAwait(false);
        }
        catch (HttpRequestException error)
        {
            if (error.StatusCode == HttpStatusCode.Unauthorized)
                await Logout();
            
            _logger.LogError(error, "{Source}:{Message}", error.Source, error.Message);
            ret = false;
        }

        ret = ret && tokens != null;
        
        if (ret)
        {
            await _uiThreadOperation.InvokeAsync(() =>
            {
                _account.Tokens.Token = tokens.Token;
                _account.Tokens.RefreshToken = tokens.RefreshToken;
            });
            
            _logger.LogInformation("Refresh tokens success. Account: {acc}", _account.Name);
        }
        else
            _logger.LogInformation("Refresh tokens failed. Account: {acc}", _account.Name);
        

        return ret;
    }
    private async Task Logout()
    {
        await _uiThreadOperation.InvokeAsync(() =>
        {
            _userStore.CurrentValue = new User();
            _userStore.SaveNow();
        });
    }
}