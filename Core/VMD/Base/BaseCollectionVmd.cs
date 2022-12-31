using ReactiveUI.Fody.Helpers;

namespace Core.VMD.Base;

public abstract class BaseCollectionVmd<T> : BaseVmd,IBaseCollectionVmd<T>
{
    [Reactive]
    public IEnumerable<T> Collection { get; protected set; }
    
    [Reactive]
    public string SearchText { get; protected set; }

    protected abstract void DoSearch(string? searchText);
}