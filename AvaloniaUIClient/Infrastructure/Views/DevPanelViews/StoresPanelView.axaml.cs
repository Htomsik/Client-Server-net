using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Infrastructure.Views.DevPanelViews;

public partial class StoresPanelView : UserControl
{
    public StoresPanelView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}