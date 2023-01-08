using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.VMD;

public abstract class BaseCollectionVmd<T> : BaseVmd,IBaseCollectionVmd<T>
{
    #region Properties

    [Reactive]
    public IEnumerable<T> Collection { get; protected set; }
    
    [Reactive]
    public string SearchText { get; set; }

    #endregion
    
    public BaseCollectionVmd()
    {
        #region Commands

        ClearSearchText = ReactiveCommand.Create(() => SearchText = string.Empty );
        
        this.WhenAnyValue(x => x.SearchText).Subscribe(DoSearch);
        
        #endregion
    }
    
    #region Commands

    public IReactiveCommand ClearSearchText { get; }

    public IReactiveCommand ClearCollection { get; protected set; }

    #endregion

    #region Methods

    protected virtual void DoSearch(string? searchText){}

    #endregion
 
}