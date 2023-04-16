using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Extensions;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Services.DialogService;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Core.VMD.DevPanelVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD;

public class MainVmd : BaseVmd
{
    #region Properties
    
    [Reactive]
    public Settings? Settings { get; set; }

    public ProjectInfo ProjectInfo { get; }
    #endregion
    
    #region Properties : VMDS
    
    public IBaseVmd? DevPanelVmd { get; }
    
    public IBaseVmd? MainMenuVmd { get; }
    
    public IBaseVmd? StatusLineVmd { get; }
    
    [Reactive]
    public ITitleVmd? TitleVmd { get; private set; }
    
    public IDialogService DialogService { get; private set; }

    #endregion

    #region Constructors

    public MainVmd(
        IDialogService dialogService,
        IStore<ITitleVmd> titleVmdStore,
        IStore<Settings> settings, 
        ProjectInfo projectInfo)
    {
        #region Subscriptions
        
        titleVmdStore.CurrentValueChangedNotifier += () => TitleVmd = titleVmdStore.CurrentValue;
        
        settings.CurrentValueChangedNotifier += () => Settings = settings.CurrentValue!;
        
        #endregion

        #region Properties initializing
        
        Settings = settings.CurrentValue;

        ProjectInfo = projectInfo;

        DialogService = dialogService;
        
        #endregion

        #region Properties initializing : VMDS

        DevPanelVmd = HostWorker.Services.GetRequiredService<DevVmd>();

        MainMenuVmd = HostWorker.Services.GetRequiredService<MainMenuVmd>();

        StatusLineVmd = HostWorker.Services.GetRequiredService<StatusLineVmd>();
        
        #endregion
    }
    #endregion
}