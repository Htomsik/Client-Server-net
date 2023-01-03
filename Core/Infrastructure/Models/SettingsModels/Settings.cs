using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.SettingsModels;

public class Settings : ReactiveObject
{
    #region Dev

    [Reactive] [JsonInclude] public int DevLogsCount { get; set; } = 50;

    [Reactive] [JsonIgnore] public bool IsDevMode { get; set; } = false;

    #endregion

}