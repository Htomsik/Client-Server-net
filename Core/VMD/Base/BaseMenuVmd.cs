using ReactiveUI;

namespace Core.VMD.Base;

public abstract class BaseMenuVmd<T> : BaseVmd,IMenuVmd<T>
{
    public IEnumerable<T> MenuItems { get; init; }

    protected IReactiveCommand? _navigationCommand;
    
}