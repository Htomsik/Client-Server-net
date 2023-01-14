using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.Services.FileService;
using Core.Infrastructure.Services.ParseService;
using Core.Infrastructure.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class SettingsFileService : BaseStoreFileService<Settings,ISaverStore<Settings,bool>>
{
    private readonly IConfiguration _configuration;
    public SettingsFileService(
        IConfiguration configuration,
        ISaverStore<Settings,bool> store, 
        IParseService parseService,
        ILogger<BaseStoreFileService<Settings,ISaverStore<Settings,bool>>> logger) : base(store, parseService, logger, "Settings.json")
    {
        _configuration = configuration;
    }

    protected override void AfterGet() 
    {
        Store.CurrentValue.IsDevMode = _configuration.GetSection("Settings:DevMode").Get<bool>();
        
        Store.SaveNowWithoutFile();
    }

    protected override void SetStoreSubscriptions(ISaverStore<Settings, bool> store)
    {
        store.CurrentValueChangedNotifier += save =>
        {
            if(save) Set();
        };
    }
}