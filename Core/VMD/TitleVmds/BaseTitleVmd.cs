using ReactiveUI;

namespace Core.VMD.TitleVmds;

public abstract class BaseTitleVmd : BaseVmd, ITitleVmd
{
    public virtual string Title => nameof(ITitleVmd);
}