using System.Net.Http.Json;
using Interfaces.Other;
using Microsoft.Extensions.Logging;
using Services.Identity;

namespace HTTPClients.Services;

public class AuthorizationHttpService<TAuthUser,TRegUser,TUser, TTokens> 
    : IAuthService<TAuthUser,TRegUser,TUser, TTokens>, IHttpService
    where TTokens : ITokens
{
    #region Fields
    
    public  HttpClient Client { get; }

    #endregion

    #region Constructors

    public AuthorizationHttpService(
        ILogger<AuthorizationHttpService<TAuthUser,TRegUser,TUser,TTokens>> logger, 
        HttpClient client)
    {
        Client = client;
    }

    #endregion

    #region Methods

    public async Task<TTokens?> Authorize(TAuthUser user, CancellationToken cancel = default)
    {
       var response = await Client.PostAsJsonAsync("Authorization", user, cancellationToken: cancel).ConfigureAwait(false);
       
       return await response
           .EnsureSuccessStatusCode()
           .Content
           .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
           .ConfigureAwait(false);
    }

    public async Task<TTokens?> Registration(TRegUser user, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("Registration", user, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    public async Task<bool> Deactivate(TTokens tokens, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("Deactivate", tokens, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    public async Task<TTokens?> RefreshTokens(TTokens tokens, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("RefreshTokens", tokens, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TTokens>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    public async Task<TUser?> Info(TTokens tokens, CancellationToken cancel = default)
    {
        var response = await Client.PostAsJsonAsync("Info", tokens, cancellationToken: cancel).ConfigureAwait(false);
       
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<TUser>(cancellationToken: cancel)
            .ConfigureAwait(false);
    }

    #endregion
}