using Core.Infrastructure.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Extensions;

public class FileExtension
{
    #region Logger

    private static ILogger? Logger => _logger ??=HostWorker.Services.GetService<ILogger<FileExtension>>();

    private static ILogger? _logger;

    #endregion
    
    #region RestoreDirectories :  Restore directory path
    
    public static bool RestoreDirectories(string path)
    {
        if (IsDirectoryExist(path))
            return true;

        try
        {
            Directory.CreateDirectory(path);
        }
        catch (Exception error)
        {
            Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);
            
            return false;
        }
        
        Logger!.LogTrace($"Directory restored: {path}","{0}:{1}", nameof(FileExtension));

        return true;
    }

    #endregion

    #region IsDirectoryExist :  Check of existing directories
    
    public static bool IsDirectoryExist(string path) =>
        string.IsNullOrEmpty(path.Trim())
        ? throw new ArgumentNullException(nameof(path))
        : Directory.Exists(path);
    

    #endregion

    #region RestoreFile : Restore file in Data/{DirectoryPath}/
    
    public static bool RestoreFile(string fileName, string directoryPath = null)
    {
        if (string.IsNullOrEmpty(directoryPath?.Trim()))
            directoryPath = Path.Combine(Directory.GetCurrentDirectory(),"Data");
        
        if (IsFileExist(directoryPath, fileName))
            return true;

        if (!IsDirectoryExist(directoryPath))
            RestoreDirectories(directoryPath);

        var path = Path.Combine(directoryPath, fileName);
        
        try
        {
            File.Create(path).Close();
        }
        catch (Exception error)
        {
            Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);
            
            return false;
        }
        
        Logger!.LogTrace($"File restored: {path}","{0}:{1}", nameof(FileExtension));

        return true;
    }

    #endregion
    
    #region IsFileExist : Check of existing files
    
    public static bool IsFileExist(string fileName, string directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath.Trim())) throw new ArgumentNullException(nameof(directoryPath));
        
        if (string.IsNullOrEmpty(fileName.Trim())) throw new ArgumentNullException(nameof(fileName));
        
        return File.Exists(Path.Combine(directoryPath,fileName));
    }

    #endregion

   
}