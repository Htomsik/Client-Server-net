using Core.Infrastructure.VMD;
using Core.VMD.DevPanelVmds.LogsVmds;


namespace Core.VMD.DevPanelVmds;

public sealed class DevVmd : BaseVmd
{
    public LogsVmd LogsPanelVmd { get; }
    
    public StoresVmd StoresPanelVmd { get; }
    
    public NotificationsVmd NotificationsPanelVmd { get; }
    
    public DevVmd(
        NotificationsVmd notificationsVmd, 
        StoresVmd storesVmd, 
        LogsVmd logsVmd)
    {
        NotificationsPanelVmd = notificationsVmd;
        StoresPanelVmd = storesVmd;
        LogsPanelVmd = logsVmd;
    }
}