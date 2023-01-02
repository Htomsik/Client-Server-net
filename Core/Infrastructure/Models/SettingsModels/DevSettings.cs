using System.Text.Json.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.SettingsModels;

public sealed class DevSettings : ReactiveObject
{ 
    [Reactive]
    [JsonInclude]
    public int LogsCount { get; set; } = 50;
    
}