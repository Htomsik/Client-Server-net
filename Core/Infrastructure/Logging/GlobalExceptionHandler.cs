using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Core.Infrastructure.Logging;

internal sealed class GlobalExceptionHandler : IObserver<Exception>
{
    private readonly ILogger _logger;

    private bool _isDevMode;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IConfiguration configuration)
    {
        _logger = logger;

        configuration
            .WhenAnyValue(x => x["Settings:DevMode:Enabled"])
            .Subscribe(_=> 
                _isDevMode = Convert.ToBoolean(configuration["Settings:DevMode:Enabled"]));
    }

    public void OnCompleted()
    {
        if(_isDevMode && Debugger.IsAttached)
            Debugger.Break();
    }

    public void OnError(Exception error)
    {
        if(_isDevMode && Debugger.IsAttached)
            Debugger.Break();
        
        _logger.LogCritical(error, "{0}:{1}", error.Source, error.Message);
    }

    public void OnNext(Exception error)
    {
        if(_isDevMode && Debugger.IsAttached)
            Debugger.Break();
        
        _logger.LogCritical(error, "{0}:{1}", error.Source, error.Message);
    }
}