using AppInfrastructure.Stores.Repositories.Collection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.ItemSelectors;

public class ItemSelector<T> : ReactiveObject, IItemSelector
{
    #region Properties
    
    public string? Name { get; } 
    
    public T Item { get; }
    
    [Reactive]
    public  bool IsAdd { get; set; }

    #endregion

    #region Constructors

    public ItemSelector(T item, ICollection<T> items): this(item)
    {
        this
            .WhenAnyValue(x => x.IsAdd)
            .Subscribe(_ =>
            {
                if (IsAdd)
                    items.Add(item);
                else
                    items.Remove(item);
            });
    }
    
    public ItemSelector(T item, ICollectionRepository<List<T>,T> items) : this(item)
    {
        this
            .WhenAnyValue(x => x.IsAdd)
            .Subscribe(_ =>
            {
                if (IsAdd)
                    items.AddIntoEnumerable(item);
                else
                    items.RemoveFromEnumerable(item);
            });
    }


    private ItemSelector(T item)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        
        Name = item.ToString();
    }

    #endregion

    #region Methods
    
    public static IEnumerable<ItemSelector<T>> CreateAll(IEnumerable<T> inputItems,ICollection<T> outputItems) 
    {
        foreach (var item in inputItems)
        {
            yield return new ItemSelector<T>(item, outputItems);
        }
    }

    public static IEnumerable<ItemSelector<T>> CreateAll(IEnumerable<T> inputItems,ICollectionRepository<List<T>, T> items)
    {
        foreach (var item in inputItems)
        {
            yield return new ItemSelector<T>(item, items);
        }
    }
    
    #endregion
    
}