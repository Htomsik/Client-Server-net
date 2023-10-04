using Avalonia;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Services.Other;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace AvaloniaUIClient.Infrastructure.Services.Other;

internal sealed class UiThemeService : IUiThemeService
{
    private readonly IUiThreadOperation _uiThreadOperation;
    
    private readonly MaterialTheme _materialThemeStyles;
    
    public UiThemeService(IUiThreadOperation uiThreadOperation)
    { 
        _uiThreadOperation = uiThreadOperation;
        _materialThemeStyles =  Application.Current!.LocateMaterialTheme<MaterialTheme>();
    }       
    public async void ChangeMode(ThemeMode mode) => await _uiThreadOperation.InvokeAsync(() =>
    {
        _materialThemeStyles.BaseTheme = (BaseThemeMode)mode;
    });
}   