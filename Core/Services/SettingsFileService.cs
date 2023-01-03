using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.Services.FileService;
using Core.Infrastructure.Services.ParseService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public sealed class SettingsFileService : BaseStoreFileService<Settings>
{
    private readonly IConfiguration _configuration;
    public SettingsFileService(
        IConfiguration configuration,
        IStore<Settings> store, 
        IParseService parseService,
        ILogger<BaseStoreFileService<Settings>> logger) : base(store, parseService, logger, "Settings.json")
    {
        _configuration = configuration;
    }

    protected override void AfterGet()
    {
        var confValue = _configuration["Settings:DevMode:IsEnabled"];
        
        Store.CurrentValue.IsDevMode = !string.IsNullOrEmpty(confValue) && Convert.ToBoolean(confValue);
        
    }
}