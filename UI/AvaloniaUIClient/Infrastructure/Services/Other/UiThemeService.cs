using Avalonia;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Services.Other;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace AvaloniaUIClient.Infrastructure.Services.Other;

internal sealed class UiThemeService : IUiThemeService
{
    private readonly MaterialTheme _materialThemeStyles;
    
    public UiThemeService()
    {
        _materialThemeStyles =  Application.Current!.LocateMaterialTheme<MaterialTheme>();
    }
    public void ChangeMode(ThemeMode mode) => _materialThemeStyles.BaseTheme = (BaseThemeMode)mode;
}