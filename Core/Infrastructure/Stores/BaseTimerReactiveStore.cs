using System.Reactive.Linq;
using Core.Infrastructure.Stores.Interfaces;
using DynamicData.Binding;
using ReactiveUI;

namespace Core.Infrastructure.Stores;

public abstract class BaseTimerReactiveStore<TValue> : BaseReactiveStore<TValue>,ISaverStore<TValue,bool> where TValue : IReactiveObject
{
    #region Properties
    
    protected abstract int InitialTimerSeconds { get; }

    private IDisposable? _timer;
    
    public event Action<long>? TimerChangeNotifier;
    
    public new event Action<bool>? CurrentValueChangedNotifier;

    #endregion
    
    #region Constructors

    public BaseTimerReactiveStore(TValue value) : base(value)
    {
        CurrentValueChangedNotifier += _ =>  base.OnCurrentValueChanged();
    }

    public BaseTimerReactiveStore() : base()
    {
        CurrentValueChangedNotifier += _ =>  base.OnCurrentValueChanged();
    }
  
    
    #endregion

    #region Methods
    
    public void SaveNow()
    {
        RebuildSubscriptions();
        
        OnCurrentValueChanged();
    }

    public void SaveNowWithoutFile()
    {
        RebuildSubscriptions();
            
        OnCurrentValueChanged(false);
    } 
    
    protected void OnCurrentValueChanged(Boolean withSawing = true)
    {
        _timer?.Dispose();
        
        CurrentValueChangedNotifier?.Invoke(withSawing);
    } 
    
    protected void OnTimerChangeNotifier(long currentSec) => TimerChangeNotifier?.Invoke(currentSec);

    private void StartTimer()
    {
        _timer?.Dispose();
        
        _timer = Observable
            .Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(1))
            .Select(currentSeconds => InitialTimerSeconds - currentSeconds)
            .TakeWhile(currentSeconds => currentSeconds >= 0)
            .Subscribe(OnTimerChangeNotifier);
    }
    
    protected override void SetValueSubscriptions()
    {
        ValueSubscriptions = CurrentValue
            .WhenAnyPropertyChanged()
            .Do(_=>StartTimer())
            .Throttle(TimeSpan.FromSeconds(InitialTimerSeconds))
            .Subscribe(_ => OnCurrentValueChanged());
    }
    
    #endregion
    
}