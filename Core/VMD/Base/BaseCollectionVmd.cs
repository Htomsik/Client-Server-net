using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.Base;

public abstract class BaseCollectionVmd<T> : BaseVmd,IBaseCollectionVmd<T>
{
    [Reactive]
    public IEnumerable<T> Collection { get; protected set; }
    
    [Reactive]
    public string SearchText { get; protected set; }
    
    #region Commands

    private IReactiveCommand ClearSearchText { get; }

    #endregion
    
    public BaseCollectionVmd()
    {
        #region Commands

        ClearSearchText = ReactiveCommand.Create(() => SearchText = string.Empty );

        #endregion
    }
    protected abstract void DoSearch(string? searchText);
}