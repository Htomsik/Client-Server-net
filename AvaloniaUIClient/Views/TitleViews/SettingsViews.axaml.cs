using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Views.TitleViews;

public partial class SettingsViews : UserControl
{
    public SettingsViews()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}