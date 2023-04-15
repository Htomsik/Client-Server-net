using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.Other;
using Microsoft.Extensions.Logging;
using Services.Identity;

namespace Core.Infrastructure.Services.AccountService;

internal sealed class AccountService : IAccountService
{
    #region Fields

    private readonly ILogger _logger;

    private readonly IAuthService<AuthUser, Tokens> _authService;
    
    private readonly IUiThreadOperation _uiThreadOperation;

    private User _account; 

    #endregion
    
    #region Constructors

    public AccountService(
        ILogger<AccountService> logger, 
        IAuthService<AuthUser, Tokens> autService,
        IStore<User> userStore,
        IUiThreadOperation uiThreadOperation)
    {
        _logger = logger;
        _authService = autService;
        _uiThreadOperation = uiThreadOperation;
        _account = userStore.CurrentValue;

        userStore.CurrentValueChangedNotifier += () =>
        {
            _account = userStore.CurrentValue;
        };
    }

    #endregion
    
    public async Task<bool> Authorization(CancellationToken cancel = default)
    {
        Tokens? tokens = null;

        bool ret = true;

        _logger.LogInformation("Authorization attempt. Account: {acc}", _account.Name);
        
        try
        {
            tokens = await _authService.Authorize(_account, cancel).ConfigureAwait(false);
        }
        catch (Exception error)
        {
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
            
            _logger.LogInformation("Authorization success. Account: {acc}", _account.Name);
        }
        else
            _logger.LogInformation("Authorization failed. Account: {acc}", _account.Name);
        

        return ret;
    }

    public async Task<bool> Registration(CancellationToken cancel = default)
    {
        Tokens? tokens = null;

        bool ret = true;

        _logger.LogInformation("Registration attempt. Account: {acc}", _account.Name);
        
        try
        {
            tokens = await _authService.Registration(_account, cancel).ConfigureAwait(false);
        }
        catch (Exception error)
        {
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
            
            _logger.LogInformation("Registration success. Account: {acc}", _account.Name);
        }
        else
            _logger.LogInformation("Registration failed. Account: {acc}", _account.Name);
        

        return ret;
    }
}