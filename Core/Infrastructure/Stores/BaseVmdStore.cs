using AppInfrastructure.Stores.DefaultStore;
using Core.VMD.Base;

namespace Core.Infrastructure.Stores;

public abstract class BaseVmdStore<TBaseVmd> : BaseLazyStore<TBaseVmd> where TBaseVmd : IBaseVmd
{
    
}