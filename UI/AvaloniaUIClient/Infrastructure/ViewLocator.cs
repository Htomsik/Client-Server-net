using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaUIClient.Infrastructure.Views;
using AvaloniaUIClient.Infrastructure.Views.DevPanelViews;
using AvaloniaUIClient.Infrastructure.Views.DevPanelViews.LogsPanel;
using AvaloniaUIClient.Views;
using AvaloniaUIClient.Views.TitleViews;
using Core.Infrastructure.Services.Other;
using Core.VMD;
using Core.VMD.DevPanelVmds;
using Core.VMD.DevPanelVmds.LogsVmds;
using Core.VMD.MenuVmd;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using MainMenuView = AvaloniaUIClient.Infrastructure.Views.MenuViews.MainMenuView;

namespace AvaloniaUIClient.Infrastructure;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Type> _vmdToViewTypes = new()
    {
        {typeof(DevVmd),typeof(DevView)},
        {typeof(LogsVmd),typeof(LogsDevView)},
        {typeof(MainMenuVmd), typeof(MainMenuView)},
        {typeof(HomeVmd),typeof(HomeView)},
        {typeof(SettingsVmd),typeof(SettingsViews)},
        {typeof(StoresVmd),typeof(StoresDevView)},
        {typeof(AboutProgramVmd),typeof(AboutProgramView)},
        {typeof(LogsSettingsVmd),typeof(LogsDevSettingsView)},
        {typeof(StatusLineVmd),typeof(StatusLineView)},
        {typeof(NotificationsVmd), typeof(NotificationView)}
    };

    private readonly ILogger _logger;

    private readonly INotificationService _notifyService;

    public ViewLocator()
    {
        _logger = App.Services.GetService<ILogger<ViewLocator>>();
        
        _notifyService = App.Services.GetService<INotificationService>();
    } 
    
    public IControl Build(object vmd)
    {
        IControl view = null!;
        
        try
        {
            view = (Control)Activator.CreateInstance(typeof(NoDataView))!;
            
            var viewType = _vmdToViewTypes[vmd.GetType()];
            
            view = (Control)Activator.CreateInstance(viewType)!;
        }
        catch(Exception error)
        {
            _logger.LogError(error, error.Message);
            _notifyService.Notify("Something went wrong");
        }
        
        return view;
    }

    public bool Match(object data) => data is ReactiveObject;
}