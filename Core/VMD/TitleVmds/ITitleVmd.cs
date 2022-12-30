using ReactiveUI;

namespace Core.VMD.TitleVmds;

public interface ITitleVmd : IReactiveObject
{
    string Title { get;}
}