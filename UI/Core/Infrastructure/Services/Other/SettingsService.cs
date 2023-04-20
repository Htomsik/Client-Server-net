using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.VMD;
using ReactiveUI;

namespace Core.Infrastructure.Services.Other;

public sealed class SettingsService : BaseVmd
{
    #region Fields

    private IDisposable _isDarkModeSubscribe;
    
    private Settings _settings;
    
    private readonly IUiThemeService _themeService;

    #endregion
    
    public SettingsService(
        IStore<Settings> settingsStore, 
        IUiThemeService themeService)
    {
        _themeService = themeService;
        
        _settings = settingsStore.CurrentValue;

        settingsStore.CurrentValueChangedNotifier += () =>
        {
            _settings = settingsStore.CurrentValue;
            
            SetSubscribes();
        };

        SetSubscribes();
    }

    #region Methods

    #region Subscribe

    private void SetSubscribes()
    {
        _isDarkModeSubscribe?.Dispose();

        _isDarkModeSubscribe =
            _settings.WhenAnyValue(x=>x.IsDarkTheme)
                .Subscribe(isDark =>
                {
                    _themeService.ChangeMode(isDark ? ThemeMode.Dark : ThemeMode.Light);
                });
    }

    #endregion

    #endregion
}