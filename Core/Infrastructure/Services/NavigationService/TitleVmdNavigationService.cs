using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.VMD.Interfaces;
using Core.VMD.TitleVmds;

namespace Core.Infrastructure.Services.NavigationService;

internal class TitleVmdsNavigationService : BaseVmdNavigationService<ITitleVmd>
{
    public TitleVmdsNavigationService(IStore<ITitleVmd> store) : base(store)
    {
        Navigate(typeof(HomeVmd));
    }
}