using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.EncryptService;
using Interfaces.Entities;

namespace Core.Infrastructure.Services.Other;

public sealed class HttpTokenService : IHttpTokenService
{
    private readonly IDecryptService _decryptService;

    private IUser _user;
    
    public HttpTokenService(
        IStore<User> userStore,
        IDecryptService decryptService)
    {
        _decryptService = decryptService;

        _user = userStore.CurrentValue;

        userStore.CurrentValueChangedNotifier += () =>
        {
            _user = userStore.CurrentValue;
        };
    }
    
    public bool SetToken(HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
        
        if (string.IsNullOrEmpty(_user.Tokens.Token))
            return false;
        
        client.DefaultRequestHeaders.Add("Authorization", _user.Tokens.Decrypt(_decryptService).Token);

        return true;
    }
}