using System.Collections.ObjectModel;
using System.Windows.Input;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Models.Menu;
using Core.Infrastructure.Services.NavigationService;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Core.VMD.MenuVmd;

public class MainMenuVmd : BaseMenuVmd<MenuParamCommandItem>
{
    #region Properties

    public ProjectInfo ProjectInfo { get; }

    #endregion

    #region Constructors

    public MainMenuVmd(ProjectInfo projectInfo)
    {
        
        #region Command Initialize

        OpenSettings = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(SettingsVmd)));

        OpenAboutProject = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(AboutProgramVmd)));
        
        NavigationCommand = ReactiveCommand.Create<Type>(
            type=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(type));
        
        #endregion
        
        #region Properties and Fields initialize

        MenuItems = new ObservableCollection<MenuParamCommandItem>
        {
            new ("Home",(ICommand)NavigationCommand!,typeof(HomeVmd)),
        };

        ProjectInfo = projectInfo;

        #endregion
    }

    #endregion
   
    #region Command

    public IReactiveCommand OpenSettings { get; }
    
    public IReactiveCommand OpenAboutProject { get; }

    #endregion
}