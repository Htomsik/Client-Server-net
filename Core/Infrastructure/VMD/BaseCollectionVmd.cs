#region

using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Core.Infrastructure.VMD;

public abstract class BaseCollectionVmd<T> : BaseVmd
{
    #region Properties
    
    public ReadOnlyObservableCollection<T>? Items => ItemsSetter;

    protected ReadOnlyObservableCollection<T>? ItemsSetter;

    [Reactive] public string? SearchText { get; set; }
    
    public bool IsInitialize { get; }

    #endregion

    #region Fields

    protected IDisposable? DisposeSubscriptions;
    
    protected readonly IObservable<Func<T, bool>> SearchFilter;
    
    #endregion
    
    #region Constructors

    public BaseCollectionVmd(ObservableCollection<T> collection)
    {
        ClearSearchText = ReactiveCommand.Create(() => SearchText = string.Empty );
        
        SearchFilter =
            this.WhenValueChanged(x => x.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .Select(SearchFilterBuilder);
        
        SetSubscriptions(collection);
        
        IsInitialize = true;
    }

    #endregion
    
    #region Commands

    public IReactiveCommand? ClearSearchText { get; } 

    public IReactiveCommand? ClearCollection { get; protected set; }

    #endregion

    #region Methods

    protected virtual Func<T, bool> SearchFilterBuilder(string? searchText)
    {
        searchText = searchText?.Trim();
        
        if (string.IsNullOrEmpty(searchText)) return _ => true;

        return x => x.ToString().Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
    }

    protected void SetSubscriptions(ObservableCollection<T> inputData)
    {
        DisposeSubscriptions?.Dispose();
        
        SetCollectionSubscriptions(inputData);
    }
    
    protected virtual void SetCollectionSubscriptions(ObservableCollection<T> inputData)
    {
        DisposeSubscriptions =
            inputData
                .ToObservableChangeSet()
                .Filter(SearchFilter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out ItemsSetter)
                .DisposeMany()
                .Subscribe();
    }

    #endregion
    
}