using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Models;
using Core.Infrastructure.Services.NavigationService;
using Core.Infrastructure.VMD;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;


namespace Core.VMD;

public class MainMenuVmd : BaseMenuVmd<MenuParamCommandItem>
{
    #region Properties

    public ProjectInfo ProjectInfo { get; }

    #endregion
    
    public MainMenuVmd(ProjectInfo projectInfo)
    {
        
        #region Command Initialize

        OpenSettings = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(SettingsVmd)));

        OpenAboutProject = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(AboutProgramVmd)));
        
        _navigationCommand = ReactiveCommand.Create<Type>(
            type=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(type));
        
        #endregion
        
        #region Properties and Fields initialize

        MenuItems = new ObservableCollection<MenuParamCommandItem>
        {
            new ("Home",(ICommand)_navigationCommand!,typeof(HomeVmd)),
        };

        ProjectInfo = projectInfo;

        #endregion

    }
    
    #region Command

    public IReactiveCommand OpenSettings { get; }
    
    public IReactiveCommand OpenAboutProject { get; }

    #endregion
}