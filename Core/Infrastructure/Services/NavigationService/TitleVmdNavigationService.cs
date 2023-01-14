using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.VMD.Interfaces;

namespace Core.Infrastructure.Services.NavigationService;

internal class TitleVmdsNavigationService : BaseVmdNavigationService<ITitleVmd>
{
    public TitleVmdsNavigationService(IStore<ITitleVmd> store) : base(store)
    {
    }
}