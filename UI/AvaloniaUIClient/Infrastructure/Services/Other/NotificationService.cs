using System;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Services.Other;
using Microsoft.Extensions.Logging;

namespace AvaloniaUIClient.Infrastructure.Services.Other;

public sealed class NotificationService : INotificationService
{
    #region Properties

    public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(7);

    #endregion
    
    #region Fields

    private IManagedNotificationManager _notificationManager;

    private readonly ILogger _logger;
    
    private readonly IUiThreadOperation _uiThreadOperation;

    #endregion

    #region Constructors

    public NotificationService(ILogger<NotificationService> logger, IUiThreadOperation uiThreadOperation)
    {
        _logger = logger;
        _uiThreadOperation = uiThreadOperation;
    }

    #endregion

    #region Methods
    
    public async void Notify(string message,string title = "Info", NotifyLevel level = NotifyLevel.Information)
    {
        await _uiThreadOperation.InvokeAsync(() =>
        {
            _notificationManager.Show(new Notification(title, message, (NotificationType)level, TimeOut));
        });
    }
  
    #endregion

    public void SetHostWindow(TopLevel? hostWindow)
    {
        if (hostWindow is null)
        {
            _logger.LogError("{window} is null, Can't create notification manager",nameof(hostWindow));
            return;
        }
        
        var notificationManager = new WindowNotificationManager((Window)hostWindow)
        {
            Position = NotificationPosition.BottomRight,
            MaxItems = 2
        };

        _notificationManager = notificationManager;
    }
}