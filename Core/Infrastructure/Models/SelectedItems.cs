using System.Collections.ObjectModel;
using Core.Infrastructure.Models.ItemSelectors;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Models;

public class SelectedItems<T, TItemSelector> : ReactiveObject where TItemSelector : ItemSelector<T>, new() where T : notnull
{
    #region Properties

    public ObservableCollection<TItemSelector> AllItems { get; }

    public IObservableCache<T, T> Filter =>
        AllItems
            .ToObservableChangeSet(x=>x.Item)
            .AutoRefresh(x=>x.IsAdd)
            .Filter(x=>x.IsAdd)
            .Transform(x => x.Item)
            .AsObservableCache();
    
    #endregion

    #region Constructors
    
    public SelectedItems(IEnumerable<T> inputItems, IEnumerable<T> outputItems) : this(inputItems)
    {
        foreach (var item in AllItems.Where(x => outputItems.Contains(x.Item)))
        {
            item.IsAdd = true;
        }
        
    }


    public SelectedItems(IEnumerable<T> inputItems) 
    {
        #region Properties initiazlie
        
        AllItems = new (inputItems.Select(item=> new TItemSelector
        {
            Item = item,
            Name = item?.ToString()
        }));
        
        #endregion
        
        #region Commands initialize

        ClearFilters = ReactiveCommand.Create(()=> 
        {
            foreach (var item in AllItems)
            {
                item.IsAdd = false;
            }
        });

        #endregion
        
    }
    
    #endregion

    #region Commands

    public IReactiveCommand ClearFilters { get; }

    #endregion
}