namespace Core.Infrastructure.VMD;

public interface IBaseCollectionVmd<out T> : IBaseVmd
{
    public IEnumerable<T> Collection { get; }

    public string SearchText { get;}
}