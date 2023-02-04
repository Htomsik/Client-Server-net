using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Core.Infrastructure.Extensions;

public class ProjectInfo
{
    #region Properties
    
    public string ProjectName => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName;
    
    public string AssemblyVerison => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;

    public string GitVersion => ThisAssembly.Git.Tag;

    public Architecture OsArchitecture => RuntimeInformation.OSArchitecture;
    
    public string OsDescription => RuntimeInformation.OSDescription;
    
    #endregion
    
}