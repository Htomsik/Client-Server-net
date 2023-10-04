using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Infrastructure.Views.DevPanelViews;

public partial class AccountDevView : UserControl
{
    public AccountDevView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}