namespace Core.Infrastructure.VMD.Interfaces;

public interface IMenuVmd<T> : IBaseVmd
{
    public IEnumerable<T>? MenuItems { get; init; }
    
}