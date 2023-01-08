using DynamicData.Binding;

namespace Core.Infrastructure.VMD;

public interface IBaseCollectionVmd<T> : IBaseVmd
{
    public ObservableCollectionExtended<T> Collection { get; }

    public string SearchText { get;}
}