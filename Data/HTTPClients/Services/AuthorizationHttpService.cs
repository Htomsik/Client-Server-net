using System.Net.Http.Json;
using Interfaces.Other;
using Microsoft.Extensions.Logging;
using Services.Identity;

namespace HTTPClients.Services;

public class AuthorizationHttpService<TUser, TTokens> : IAuthService<TUser, TTokens>
    where TTokens : ITokens
{
    #region Fields
    
    private readonly HttpClient _client;

    #endregion

    #region Constructors

    public AuthorizationHttpService(
        ILogger<AuthorizationHttpService<TUser,TTokens>> logger, 
        HttpClient client)
    {
 
        _client = client;
    }

    #endregion

    #region Methods

    public async Task<TTokens?> Authorize(TUser user, CancellationToken cancel = default)
    {
       var response = await _client.PostAsJsonAsync("Authorization", user, cancellationToken: cancel).ConfigureAwait(false);
       
       return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<TTokens?> Registration(TUser user, CancellationToken cancel = default)
    {
        var response = await _client.PostAsJsonAsync("Registration", user, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
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