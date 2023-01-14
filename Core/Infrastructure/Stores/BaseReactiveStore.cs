using AppInfrastructure.Stores.DefaultStore;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Stores;

public abstract class  BaseReactiveStore<TValue> : BaseLazyStore<TValue> where TValue : IReactiveObject
{

    protected IDisposable? ValueSubscriptions;
    
    public override TValue? CurrentValue
    {
        get => (TValue?)_currentValue.Value;
        set
        {
            _currentValue = new Lazy<object?>(()=> value);
            
            if (value == null || value.Equals(null))
                OnCurrentValueDeleted();

            SetSubscriptions();
        }
      
    }
    
    #region Constructors

    public BaseReactiveStore(TValue value):base(value) {}

    public BaseReactiveStore() : base() {}

    #endregion

    #region Methods

    protected void SetSubscriptions()
    {
        ValueSubscriptions?.Dispose();
        
        SetValueSubscriptions();
    }

    protected virtual void SetValueSubscriptions()
    {
        ValueSubscriptions = CurrentValue?
            .WhenAnyPropertyChanged()
            .Subscribe(_ => OnCurrentValueChanged());
    }
    
    protected virtual void RebuildSubscriptions()
    {
        ValueSubscriptions?.Dispose();
        
        SetValueSubscriptions();
    }

    #endregion
}