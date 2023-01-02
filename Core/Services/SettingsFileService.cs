using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.SettingsModels;
using Core.Infrastructure.Services.FileService;
using Core.Infrastructure.Services.ParseService;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class SettingsFileService : BaseStoreFileService<Settings>
{
    public SettingsFileService(IStore<Settings> store, IParseService parseService,
        ILogger<BaseStoreFileService<Settings>> logger) : base(store, parseService, logger, "Settings.json")
    {
    }
}