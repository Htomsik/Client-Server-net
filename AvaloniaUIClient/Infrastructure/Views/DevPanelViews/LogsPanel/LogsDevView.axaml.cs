using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Infrastructure.Views.DevPanelViews.LogsPanel;

public partial class LogsDevView : UserControl
{
    public LogsDevView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}