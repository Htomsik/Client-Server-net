using System.Reactive.Linq;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Services.ParseService;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Services.FileService;

public abstract class BaseStoreFileService<T> : ReactiveObject, IFileService
{
    #region Fields

    protected readonly IStore<T> Store;

    private readonly IParseService _parseService;

    private readonly ILogger _logger;

    private readonly string _fileName;
    
    #endregion
    
    #region Constructors

    protected BaseStoreFileService(IStore<T> store,IParseService parseService,ILogger<BaseStoreFileService<T>> logger,string fileName) 
    {
        Store = store ?? throw new ArgumentNullException(nameof(store));

        _parseService = parseService ?? throw new ArgumentNullException(nameof(parseService));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        Store.CurrentValueChangedNotifier += Set;
    }
    
    #endregion

    #region Methods
    
    public void Get()
    {
        if (!FileExtension.IsFileExist(_fileName))
        {
            _logger.LogWarning($"{nameof(Get)}:{_fileName} doesn't exists");
            return;
        }
        
        var nonSerialize = FileExtension.Read(_fileName);

        if (string.IsNullOrEmpty(nonSerialize.Trim()))
        {
            _logger.LogWarning($"{nameof(Get)}:{_fileName} Data is null");
            return;
        }
        
        var deSerialize =  _parseService.DeSerialize<T>(nonSerialize);

        if (deSerialize == null)
        {
            _logger.LogWarning($"{nameof(Get)}:{_fileName} Data is null");
            return;
        }
        
        Store.CurrentValue = deSerialize;
            
        _logger.LogInformation($"{nameof(Get)}:Data restored from {_fileName}");

        AfterGet();
    }

    protected virtual void AfterGet() {}
    
    public async void Set()
    {
        if (Store.CurrentValue is null)
        {
            _logger.LogWarning($"{nameof(Set)}:Store is null");
            return;
        } 
        
        var serialized = _parseService.Serialize(Store.CurrentValue);
        
        var isSaved = await FileExtension.WriteAsync(serialized, _fileName);
        
        if(isSaved)
            _logger.LogInformation($"{nameof(Set)}:{_fileName} saved confirmed");
        else
            _logger.LogError($"{nameof(Set)}:{_fileName} saved failed");

        AfterSet();
    }
    
    protected virtual void AfterSet() {}

    #endregion
}