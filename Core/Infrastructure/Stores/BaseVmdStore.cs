using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.VMD;

namespace Core.Infrastructure.Stores;

public abstract class BaseVmdStore<TBaseVmd> : BaseLazyStore<TBaseVmd> where TBaseVmd : IBaseVmd
{
    
}