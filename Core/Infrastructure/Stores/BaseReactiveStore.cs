using AppInfrastructure.Stores.DefaultStore;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Stores;

public abstract class  BaseReactiveStore<TValue> : BaseLazyStore<TValue> where TValue : IReactiveObject
{
    
    public override TValue? CurrentValue
    {
        get => (TValue?)_currentValue.Value;
        set
        {
            _currentValue = new Lazy<object?>(()=> value);
            
            CurrentValue?
                .WhenAnyPropertyChanged()
                .Subscribe(_ => OnCurrentValueChanged());
            
            OnCurrentValueChanged();
        }
      
    }

    
    #region Constructors

    public BaseReactiveStore(TValue value):base(value) {}

    public BaseReactiveStore() : base() {}

    #endregion
}