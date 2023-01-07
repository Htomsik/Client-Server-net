using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.VMD;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Infrastructure.Services.NavigationService;

public abstract class BaseVmdNavigationService<TBaseVMd> : BaseTypeNavigationService<TBaseVMd> where TBaseVMd : IBaseVmd
{
    public BaseVmdNavigationService(IStore<TBaseVMd> store) 
        : base(store, 
            type =>(TBaseVMd)HostWorker.Services.GetRequiredService(type))
    {
    }
    
}