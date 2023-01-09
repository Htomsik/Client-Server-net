using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Models.ItemSelectors;

public class ItemSelector<T> : ReactiveObject, IItemSelector
{
    #region Properties
    
    public string? Name { get; set; } 
    
    public T Item { get; set; }
    
    [Reactive]
    public  bool IsAdd { get; set; }
    
    #endregion

    #region Constructors

    public ItemSelector() { }
    
    private ItemSelector(T item)
    {
        Item = item ?? throw new ArgumentNullException(nameof(item));
        
        Name = item.ToString();
    }

    #endregion
    
}