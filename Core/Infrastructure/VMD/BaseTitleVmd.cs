namespace Core.Infrastructure.VMD;

public abstract class BaseTitleVmd : BaseVmd, ITitleVmd
{
    public virtual string Title => nameof(ITitleVmd);
}