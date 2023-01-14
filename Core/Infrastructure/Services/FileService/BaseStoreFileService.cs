using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Services.ParseService;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Core.Infrastructure.Services.FileService;

public abstract class BaseStoreFileService<T,TStore> : ReactiveObject, IFileService where TStore : IStore<T>
{
    #region Fields

    protected readonly TStore Store;

    private readonly IParseService _parseService;

    private readonly ILogger _logger;

    private readonly string _fileName;
    
    #endregion
    
    #region Constructors

    protected BaseStoreFileService(TStore store,IParseService parseService,ILogger logger,string fileName) 
    {
        Store = store ?? throw new ArgumentNullException(nameof(store));

        _parseService = parseService ?? throw new ArgumentNullException(nameof(parseService));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        SetStoreSubscriptions(store);
    }
    
    #endregion

    #region Methods

    protected virtual void SetStoreSubscriptions(TStore store) => store.CurrentValueChangedNotifier += Set; 
    
    public void Get()
    {
        if (!FileExtension.IsFileExist(_fileName))
        {
            _logger.LogWarning(StringExtensions.MessageTemplateBuilder($"{_fileName} doesn't exists"));
            return;
        }
        
        var nonSerialize = FileExtension.Read(_fileName);

        if (string.IsNullOrEmpty(nonSerialize.Trim()))
        {
            _logger.LogWarning(StringExtensions.MessageTemplateBuilder($"{_fileName} Data is null"));
            return;
        }
        
        var deSerialize =  _parseService.DeSerialize<T>(nonSerialize);

        if (deSerialize == null)
        {
            _logger.LogWarning(StringExtensions.MessageTemplateBuilder($"{_fileName} Data is null"));
            return;
        }
        
        Store.CurrentValue = deSerialize;
            
        _logger.LogInformation(StringExtensions.MessageTemplateBuilder($"Data restored from {_fileName}"));

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
            _logger.LogInformation(StringExtensions.MessageTemplateBuilder($"{_fileName} save confirmed"));
        else
            _logger.LogError(StringExtensions.MessageTemplateBuilder($"{_fileName} save failed"));

        AfterSet();
    }
    
    protected virtual void AfterSet() {}

    #endregion
}