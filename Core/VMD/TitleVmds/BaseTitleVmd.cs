using ReactiveUI;

namespace Core.VMD.TitleVmds;

public abstract class BaseTitleVmd : ReactiveObject, ITitleVmd
{
    public virtual string Title => nameof(ITitleVmd);
}