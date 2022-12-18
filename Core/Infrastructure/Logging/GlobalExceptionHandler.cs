using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Core.Infrastructure.Logging;

internal sealed class GlobalExceptionHandler : IObserver<Exception>
{
    private readonly ILogger _logger;

    private bool _isDebugMode;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IConfiguration configuration)
    {
        _logger = logger;

        configuration
            .WhenAnyValue(x => x["Settings:DebugMode:IsDebugMode"])
            .Subscribe(_=> 
                _isDebugMode = Convert.ToBoolean(configuration["Settings:DebugMode:IsDebugMode"]));
    }

    public void OnCompleted()
    {
        if(_isDebugMode && Debugger.IsAttached)
            Debugger.Break();
    }

    public void OnError(Exception error)
    {
        if(_isDebugMode && Debugger.IsAttached)
            Debugger.Break();
        
        _logger.LogCritical(error, "{0}:{1}", error.Source, error.Message);
    }

    public void OnNext(Exception error)
    {
        if(_isDebugMode && Debugger.IsAttached)
            Debugger.Break();
        
        _logger.LogCritical(error, "{0}:{1}", error.Source, error.Message);
    }
}