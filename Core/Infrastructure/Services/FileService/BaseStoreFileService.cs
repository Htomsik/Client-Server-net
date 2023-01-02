using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Services.ParseService;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Services.FileService;

public abstract class BaseStoreFileService<T> : IFileService
{
    #region Fields

    private readonly IStore<T> _store;

    private readonly IParseService _parseService;

    private readonly ILogger _logger;

    private readonly string _fileName;

    #endregion
    
    #region Constructors

    protected BaseStoreFileService(IStore<T> store,IParseService parseService,ILogger<BaseStoreFileService<T>> logger,string fileName) 
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));

        _parseService = parseService ?? throw new ArgumentNullException(nameof(parseService));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        
        _store.CurrentValueChangedNotifier += Set;
    }
    
    #endregion

    #region Methods
    
    public void Get()
    {
        if (!FileExtension.IsFileExist(_fileName))
            _logger.LogError($"File {nameof(_fileName)} doesn't exists");

        var nonSerialize = FileExtension.Read(_fileName);

        if (string.IsNullOrEmpty(nonSerialize.Trim()))
        {
            _logger.LogTrace($"{nameof(Set)}:Data is null");
            return;
        }

        _store.CurrentValue = _parseService.DeSerialize<T>(nonSerialize)!;
        
        _logger.LogInformation("Data restored from File");
    }

    public async void Set()
    {
        if (_store.CurrentValue is null)
        {
            _logger.LogTrace($"{nameof(Set)}:Store is null");
            return;
        } 
        
        var serialized = _parseService.Serialize(_store.CurrentValue);
        
        var isSaved = await FileExtension.WriteAsync(serialized, _fileName);
        
        if(isSaved)
            _logger.LogInformation("Data saved into File");
    }

    #endregion
}