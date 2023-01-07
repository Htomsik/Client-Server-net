
using System.Xml.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace Core.Infrastructure.Models.SettingsModels;

public class Settings : ReactiveObject
{
    #region Dev

     [Reactive] public int DevLogsCount { get; set; } = 50;
     
     [XmlIgnore, Reactive] public bool IsDevMode { get; set; } = false;
     
    #endregion
}