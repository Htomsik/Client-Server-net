using Core.Infrastructure.Hosting;
using Core.Infrastructure.Services;
using Core.VMD.Base;
using Core.VMD.TitleVmds;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;


namespace Core.VMD;

public class MainMenuVmd : BaseVmd
{
    private IReactiveCommand OpenSettings { get; }
    public MainMenuVmd()
    {
        OpenSettings = ReactiveCommand.Create(
            ()=> HostWorker.Services.GetService<BaseVmdNavigationService<ITitleVmd>>()!.Navigate(typeof(SettingsVmd)));
    }
}