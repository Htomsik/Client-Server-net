using Core.Infrastructure.Services.Other;
using Core.Infrastructure.VMD;
using ReactiveUI;

namespace Core.VMD.DevPanelVmds;

public sealed class NotificationsVmd : BaseVmd
{
    #region Constructors

    public NotificationsVmd(INotificationService notifyService)
    {
        Notify = ReactiveCommand.Create(()=>notifyService.Notify("TEST NOTIFICATION"));
    }

    #endregion
    
    public IReactiveCommand Notify { get; } 
}