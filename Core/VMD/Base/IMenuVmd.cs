namespace Core.VMD.Base;

public interface IMenuVmd<T> : IBaseVmd
{
    public IEnumerable<T> MenuItems { get; init; }
    
}