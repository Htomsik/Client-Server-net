using Core.Infrastructure.Extensions;
using Core.Infrastructure.VMD;

namespace Core.VMD;

public class AboutProgramVmd : BaseTitleVmd
{
    #region Properties

    public override string Title => "About program";
    
    private  ProjectInfo ProjectInfo { get;}
    
    #endregion

    #region Constructors

    public AboutProgramVmd(ProjectInfo projectInfo)
    {
        ProjectInfo = projectInfo;
    }

    #endregion
}