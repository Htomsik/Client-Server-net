using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.FileService;
using Core.Infrastructure.Services.ParseService;
using Core.Infrastructure.Stores.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class UserFleService : BaseStoreFileService<User, ISaverStore<User, bool>>
{
    public UserFleService(
        ISaverStore<User, bool> store, 
        IParseService parseService, 
        ILogger<UserFleService> logger) : base(store, parseService, logger, "User.Json")
    {
    }
    
    protected override void SetStoreSubscriptions(ISaverStore<User, bool> store)
    {
        store.CurrentValueChangedNotifier += save =>
        {
            if(save) Set();
        };
    }
}