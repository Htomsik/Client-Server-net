
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;


namespace Core.Infrastructure.Models.SettingsModels;

public class Settings : ReactiveObject
{
    #region Dev

     [Reactive] public int DevLogsCount { get; set; } = 50;
     
     [XmlIgnore, Reactive] public bool IsDevMode { get; set; } = false;
     
     [XmlIgnore,Reactive] public ObservableCollection<LogEventLevel> ShowedLogLevels { get; set; } = new() {LogEventLevel.Information,LogEventLevel.Error};
     
     #endregion
     
   
}