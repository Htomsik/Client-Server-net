using System.Text.Json.Serialization;
using ReactiveUI;

namespace Core.Infrastructure.Models.SettingsModels;

public class Settings : ReactiveObject
{
    [JsonInclude]
    public DevSettings DevSettings { get; set; }
}