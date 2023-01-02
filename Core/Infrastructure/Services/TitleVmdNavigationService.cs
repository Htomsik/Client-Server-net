using AppInfrastructure.Stores.DefaultStore;
using Core.VMD.Base;

namespace Core.Infrastructure.Services;

public class TitleVmdsNavigationService : BaseVmdNavigationService<ITitleVmd>
{
    public TitleVmdsNavigationService(IStore<ITitleVmd> store) : base(store)
    {
    }
}