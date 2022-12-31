using Core.Infrastructure.Hosting;
using Core.VMD.Base;
using Microsoft.Extensions.DependencyInjection;


namespace Core.VMD.DevPanelVmds;

public sealed class DevVmd : BaseVmd
{
    public LogsVmd? LogsPanelVmd { get; set; }
    
    public DevVmd()
    {
        LogsPanelVmd = HostWorker.Services.GetRequiredService<LogsVmd>();
    }
}