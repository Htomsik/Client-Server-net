using System.Net.Http.Json;
using Interfaces.Other;
using Microsoft.Extensions.Logging;
using Services.Identity;

namespace HTTPClients.Services;

public class AuthorizationHttpService<TAuthUser,TRegUser, TTokens> : IAuthService<TAuthUser,TRegUser, TTokens>
    where TTokens : ITokens
{
    #region Fields
    
    private readonly HttpClient _client;

    #endregion

    #region Constructors

    public AuthorizationHttpService(
        ILogger<AuthorizationHttpService<TAuthUser,TRegUser,TTokens>> logger, 
        HttpClient client)
    {
 
        _client = client;
    }

    #endregion

    #region Methods

    public async Task<TTokens?> Authorize(TAuthUser user, CancellationToken cancel = default)
    {
       var response = await _client.PostAsJsonAsync("Authorization", user, cancellationToken: cancel).ConfigureAwait(false);
       
       return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<TTokens?> Registration(TRegUser user, CancellationToken cancel = default)
    {
        var response = await _client.PostAsJsonAsync("Registration", user, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    public async Task<bool> Deactivate(TTokens tokens, CancellationToken cancel = default)
    {
        var response = await _client.PostAsJsonAsync("Deactivate", tokens, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    public async Task<TTokens?> RefreshTokens(TTokens tokens, CancellationToken cancel = default)
    {
        var response = await _client.PostAsJsonAsync("RefreshTokens", tokens, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }
    #endregion
}