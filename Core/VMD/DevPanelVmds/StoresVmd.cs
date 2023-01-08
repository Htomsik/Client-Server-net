using System.Collections.ObjectModel;
using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models;
using Core.Infrastructure.VMD;

namespace Core.VMD.DevPanelVmds;

public class StoresVmd : BaseCollectionVmd<ReflectionNode>
{
    public StoresVmd(IEnumerable<IStore> stores) : base(new ObservableCollection<ReflectionNode>(stores.Select(x => new ReflectionNode(x.GetType())))) { }

    protected override Func<ReflectionNode, bool> SearchFilterBuilder(string? searchText)
    {
        searchText = searchText?.Trim();
        
        if (string.IsNullOrEmpty(searchText)) return x => true;

        return x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
    }
}