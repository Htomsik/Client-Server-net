using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaUIClient.Infrastucture.Views;
using Core.VMD.DevPanelVmds;
using ReactiveUI;

namespace AvaloniaUIClient;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        var name = data.GetType().Name!.Replace("Vmd", "View");

        var fullName = typeof(DevPanelView).FullName.Replace(typeof(DevPanelView).Name,name);
        
        var type = Type.GetType(fullName);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ReactiveObject;
    }
}