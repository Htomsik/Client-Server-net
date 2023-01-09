using Core.Infrastructure.Hosting;
using Core.Infrastructure.VMD;
using Core.VMD.DevPanelVmds.LogsVmds;
using Microsoft.Extensions.DependencyInjection;


namespace Core.VMD.DevPanelVmds;

public sealed class DevVmd : BaseVmd
{
    public LogsVmd LogsPanelVmd { get; }
    
    public StoresVmd StoresPanelVmd { get; }
    
    public DevVmd()
    {
        LogsPanelVmd = HostWorker.Services.GetRequiredService<LogsVmd>();
        
        StoresPanelVmd = HostWorker.Services.GetRequiredService<StoresVmd>();
    }
}