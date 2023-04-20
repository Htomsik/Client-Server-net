using Core.Infrastructure.VMD;
using Core.VMD.DevPanelVmds.LogsVmds;

namespace Core.VMD.DevPanelVmds;

public sealed class DevVmd : BaseVmd
{
    public LogsVmd LogsPanelVmd { get; }
    
    public StoresVmd StoresPanelVmd { get; }
    
    public AccountDevVmd AccountDevVmd { get; }
    public NotificationsVmd NotificationsPanelVmd { get; }
    
    public DevVmd(
        LogsVmd logsVmd, 
        StoresVmd storesVmd, 
        AccountDevVmd accountDevVmd,
        NotificationsVmd notificationsVmd)
    {
        LogsPanelVmd = logsVmd;

        StoresPanelVmd = storesVmd;

        AccountDevVmd = accountDevVmd;
        
        NotificationsPanelVmd = notificationsVmd;
    }
}