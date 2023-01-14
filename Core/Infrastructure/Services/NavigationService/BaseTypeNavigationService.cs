using AppInfrastructure.Services.StoreServices.Parametrize;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.VMD.Interfaces;

namespace Core.Infrastructure.Services.NavigationService;

public abstract class BaseTypeNavigationService<TBaseVmd> : BaseLazyParamFullNavigationService<Type,TBaseVmd> where TBaseVmd : IBaseVmd
{
    public BaseTypeNavigationService(IStore<TBaseVmd> store, Func<Type, TBaseVmd> navigationFunc) : base(store, navigationFunc)
    {
    }
}