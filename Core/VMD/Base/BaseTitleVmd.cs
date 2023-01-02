namespace Core.VMD.Base;

public abstract class BaseTitleVmd : BaseVmd, ITitleVmd
{
    public virtual string Title => nameof(ITitleVmd);
}