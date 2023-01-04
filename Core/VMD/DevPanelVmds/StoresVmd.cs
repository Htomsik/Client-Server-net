using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models;
using Core.Infrastructure.VMD;
using DynamicData;

namespace Core.VMD.DevPanelVmds;

public class StoresVmd : BaseCollectionVmd<ReflectionNode>
{
    private readonly IEnumerable<ReflectionNode>? _stores;

    public StoresVmd(IEnumerable<IStore> stores)
    {
        _stores = stores.Select(x => new ReflectionNode(x.GetType()));
        
        Collection = _stores;
    }

    protected override void DoSearch(string? searchText)
    {
        Collection = _stores?.Where(x=> 
            !string.IsNullOrEmpty(searchText) 
                ? x.Name.Contains(searchText,StringComparison.CurrentCultureIgnoreCase) : true)!;
    }
}