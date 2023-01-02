using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Models;
using Core.Infrastructure.Services.NavigationService;
using Core.VMD.Base;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;


namespace Core.VMD;

public class MainMenuVmd : BaseMenuVmd<MenuParamCommandItem>
{
    public MainMenuVmd()
    {
        
        #region Command Initialize

        OpenSettings = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(SettingsVmd)));
        
        _navigationCommand = ReactiveCommand.Create<Type>(
            type=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(type));
        
        #endregion
        
        #region Properties and Fields initialize

        MenuItems = new ObservableCollection<MenuParamCommandItem>
        {
            new ("Home",(ICommand)_navigationCommand!,typeof(HomeVmd)),
        };

        #endregion
        
    }
    
    #region Command

    private IReactiveCommand OpenSettings { get; }

    #endregion
}