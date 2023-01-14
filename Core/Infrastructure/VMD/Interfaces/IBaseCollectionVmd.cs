using DynamicData.Binding;

namespace Core.Infrastructure.VMD.Interfaces;

public interface IBaseCollectionVmd<T> : IBaseVmd
{
    public ObservableCollectionExtended<T> Collection { get; }

    public string SearchText { get;}
}