using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaUIClient.Infrastucture.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace AvaloniaUIClient.Infrastucture;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Type> _vmdToViewTypes = new()
    {
      //  {typeof(DevPanelVmd),typeof(DevPanelView)}
    };

    private ILogger? _logger;
    
    public ViewLocator() => _logger = App.Services.GetService<ILogger<ViewLocator>>();
    
    public IControl Build(object vmd)
    {
        Type? viewType = null;
        
        IControl? view = (Control)Activator.CreateInstance(typeof(NoDataView))!;

        try
        {
            viewType = _vmdToViewTypes[vmd.GetType()];
            
            view = (Control)Activator.CreateInstance(viewType)!;
        }
        catch(Exception error)
        {
            _logger?.LogError(error, "{0}:{1}", error.Source, error.Message);
        }
        
        return view;
    }

    public bool Match(object data) => data is ReactiveObject;

}