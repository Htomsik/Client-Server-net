namespace Core.Infrastructure.VMD;

public interface IMenuVmd<T> : IBaseVmd
{
    public IEnumerable<T> MenuItems { get; init; }
    
}