using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Services.ParseService;

public class ParseService : IParseService
{

    #region Fields

    private readonly ILogger _logger;

    #endregion
    
    #region Constructors

    public ParseService(ILogger<ParseService> logger) =>
        _logger = logger;
    
    #endregion
    
    #region Methods

    public string Serialize(object nonSerialized)
    {
        string serialized = String.Empty;

        try
        {
            serialized = fastJSON.JSON.ToNiceJSON(nonSerialized);
        }
        catch (Exception error)
        {
            _logger.LogError(error, "{0}:{1}", error.Source, error.Message);
        }
        
        _logger.LogTrace($"{nameof(Serialize)}: Object serialized");

        return serialized;
    }
    
    public T? DeSerialize<T>(string serialized)
    {
        T? deSerialized = default;
        
        try
        {
            deSerialized = fastJSON.JSON.ToObject<T>(serialized);
        }
        catch (Exception error)
        {
            _logger.LogError(error, "{0}:{1}", error.Source, error.Message);
        }

        _logger.LogTrace($"{nameof(DeSerialize)}: Object deserialized");
        
        return deSerialized;
    }

    #endregion
    
}