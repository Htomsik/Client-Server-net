using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Hosting;
using Core.VMD.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Services;

public abstract class BaseVmdNavigationService<TBaseVMd> : BaseTypeNavigationService<TBaseVMd> where TBaseVMd : IBaseVmd
{
    public BaseVmdNavigationService(IStore<TBaseVMd> store) : base(store,getVmdFromService)
    {
    }

    private static TBaseVMd getVmdFromService(Type vmdType) =>
        (TBaseVMd)HostWorker.Services.GetRequiredService(vmdType);
        

}