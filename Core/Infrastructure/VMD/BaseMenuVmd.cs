using ReactiveUI;

namespace Core.Infrastructure.VMD;

public abstract class BaseMenuVmd<T> : BaseVmd,IMenuVmd<T>
{
    public IEnumerable<T> MenuItems { get; init; }

    protected IReactiveCommand? _navigationCommand;
    
}