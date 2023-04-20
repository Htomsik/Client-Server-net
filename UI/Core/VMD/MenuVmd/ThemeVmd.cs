using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Infrastructure.Models.Menu;
using Core.Infrastructure.Models.Other;
using Core.Infrastructure.Services.Other;
using Core.Infrastructure.VMD;
using ReactiveUI;

namespace Core.VMD.MenuVmd;

public sealed class ThemeVmd : BaseMenuVmd<MenuParamCommandItem>
{
    #region Constructors

    public ThemeVmd(IUiThemeService themeService)
    {
        ChangeMode = ReactiveCommand.Create<ThemeMode>(
            themeService.ChangeMode);
        
        MenuItems = new ObservableCollection<MenuParamCommandItem>
        {
            new ("Dark",(ICommand)ChangeMode!,ThemeMode.Dark),
            new ("Light",(ICommand)ChangeMode!,ThemeMode.Light)
        };
    }

    #endregion

    #region Commands

    public IReactiveCommand ChangeMode { get; }

    #endregion
}