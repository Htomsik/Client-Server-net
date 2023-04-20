using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Infrastructure.Views.DevPanelViews;

public partial class StoresDevView : UserControl
{
    public StoresDevView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}