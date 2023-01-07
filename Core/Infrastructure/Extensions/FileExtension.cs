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
        var ret = IsDirectoryExist(path);
        
        if(!ret)
            try
            {
                Directory.CreateDirectory(path);

                ret = true;
            }
            catch (Exception error)
            {
                Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);

                ret = false;
            }
        
        if(ret)
            Logger!.LogWarning($"Directory restored: {path}","{0}:{1}", nameof(FileExtension));

        return ret;
    }

    #endregion

    #region IsDirectoryExist :  Check of existing directories
    
    public static bool IsDirectoryExist(string path) =>
        string.IsNullOrEmpty(path.Trim())
        ? throw new ArgumentNullException(nameof(path))
        : Directory.Exists(path);
    

    #endregion

    #region RestoreFile : Restore file directoryPath/fileName
    
    public static bool RestoreFile(string fileName, string? directoryPath = null)
    {
        bool ret = true; 
        
        if (string.IsNullOrEmpty(directoryPath?.Trim()))
            directoryPath = Path.Combine(Directory.GetCurrentDirectory(),"Data");
        
        if (IsFileExist(directoryPath, fileName))
            ret = true;

        if (!IsDirectoryExist(directoryPath))
            ret = RestoreDirectories(directoryPath);

        var path = Path.Combine(directoryPath, fileName);
       
        if(ret)
            try
            {
                File.Create(path).Close();
            }
            catch (Exception error)
            {
                Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);
                
                return false;
            }
        
        if(ret)
            Logger!.LogWarning($"File restored: {path}","{0}:{1}", nameof(FileExtension));

        return ret;
    }

    #endregion

    #region WriteAsync : Save string data into file

    public static async Task<bool> WriteAsync(string data, string fileName, string? directoryPath = null, bool restoreFile = true)
    {
        var ret = true;
        
        if (string.IsNullOrEmpty(data.Trim())) throw new ArgumentNullException(nameof(data));
        
        if (string.IsNullOrEmpty(fileName.Trim())) throw new ArgumentNullException(nameof(fileName));

        if (string.IsNullOrEmpty(directoryPath))
            directoryPath = Path.Combine(Directory.GetCurrentDirectory(),"Data");
        
        if (restoreFile)
            ret = RestoreFile(fileName,directoryPath);
        
        string path = Path.Combine(directoryPath, fileName);
        
        if(ret)
            try
            {
                await using StreamWriter writer = new StreamWriter(path, false);
                
                await writer.WriteLineAsync(data);
            }
            catch (Exception error)
            {
                Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);

                ret = false;
            }
        
        if(ret)
            Logger!.LogWarning($"Data saved in {path}","{0}:{1}", nameof(WriteAsync));

        return ret;
    }

    #endregion

    #region ReadAsync : Get string data from file

    public static string Read(string fileName, string? directoryPath = null)
    {
        bool ret = true;

        string textFromFile = String.Empty;
        
        if (string.IsNullOrEmpty(fileName.Trim())) throw new ArgumentNullException(nameof(fileName));

        if (string.IsNullOrEmpty(directoryPath))
            directoryPath = Path.Combine(Directory.GetCurrentDirectory(),"Data");

        var path = Path.Combine(directoryPath, fileName);

        ret = IsFileExist(fileName, directoryPath);
        
        if(!ret)
            _logger!.LogError($"File {path} doesn't exists");

        if(ret)
            try
            {
                using StreamReader reader = new StreamReader(path);

                while (reader.ReadLine() is { } line)
                {
                    textFromFile += line;
                }
            }
            catch (Exception error)
            {
                Logger!.LogError(error, "{0}:{1}", error.Source, error.Message);

                ret = false;
            }
        
        if(ret)
            Logger!.LogWarning($"Data restored from {path}","{0}:{1}", nameof(Read));

        return textFromFile;
    }

    #endregion
    
    #region IsFileExist : Check of existing files
    
    public static bool IsFileExist(string fileName, string directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath.Trim())) throw new ArgumentNullException(nameof(directoryPath));
        
        if (string.IsNullOrEmpty(fileName.Trim())) throw new ArgumentNullException(nameof(fileName));
        
        return File.Exists(Path.Combine(directoryPath,fileName));
    }
    
    public static bool IsFileExist(string fileName)
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(),"Data");

        return IsFileExist(fileName, directoryPath);
    }

    #endregion
    
}