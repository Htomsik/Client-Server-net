using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog.Events;

namespace Core.VMD.DevPanelVmds;

public sealed class DevVmd : ReactiveObject
{
    public LogsVmd? LogsPanelVmd { get; set; }
    
    public DevVmd()
    {
        LogsPanelVmd = HostWorker.Services.GetRequiredService<LogsVmd>();
    }
}