using System.Xml.Serialization;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.Infrastructure.Models.Settings;

public class Settings : ReactiveObject
{
    #region Properties

    #region Dev
    
    [XmlIgnore, Reactive] public bool IsDevMode { get; set; } = false;
    [Reactive] public int DevLogsCount { get; set; } = 50;
     
    #region ShowedLogLevels

    private IDisposable _showedLogLevelsDispose;
     
    [Reactive] 
    public ObservableCollectionExtended<LogEventLevel> ShowedLogLevels { get; set; }  = new (new List<LogEventLevel>
    { 
        LogEventLevel.Error, LogEventLevel.Information
    });

    #endregion
     
    #endregion

    #endregion
    
    #region Constructors
    public Settings()
    {
        SetShowedLogLevelsSubscribes();

        this
            .WhenAnyValue(x => x.ShowedLogLevels)
            .Subscribe(_ => SetShowedLogLevelsSubscribes());
    }
    #endregion
    
    #region Methods
    private void SetShowedLogLevelsSubscribes()
    {
        _showedLogLevelsDispose?.Dispose();
         
        _showedLogLevelsDispose = 
            ShowedLogLevels
                .ToObservableChangeSet()
                .Subscribe(_ => this.RaisePropertyChanged(nameof(ShowedLogLevels)));
    }
    #endregion
}