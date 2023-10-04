using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUIClient.Views.TitleViews;

public partial class AccountView : UserControl
{
    public AccountView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}