using System.Diagnostics;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

internal sealed class  DataDbInitializer : IDbInitializer
{
    #region Fields

    private readonly DataDb _db;

    private readonly IConfiguration _configuration;

    private readonly ILogger _logger;

    #endregion

    #region Constants

    private const string ResourceDirName = "Resources";

    #endregion

    #region Constructors

    public DataDbInitializer(
        DataDb db, 
        IConfiguration configuration, 
        ILogger<DataDbInitializer> logger)
    {
        _db = db;
        _configuration = configuration;
        _logger = logger;
    }

    #endregion

    #region Methods

    private bool RestoreResourcesDirectory()
    {
        bool ret = true;
        
        var resourceDirectory = Path.Combine(Directory.GetCurrentDirectory(), ResourceDirName);

        if (!Directory.Exists(resourceDirectory))
        {
            _logger.LogInformation("Start restore directory {directory}. Path: {path}", ResourceDirName,resourceDirectory);
            
            try
            {
                Directory.CreateDirectory(resourceDirectory);
                
                _logger.LogInformation("Restore directory {directory} success", ResourceDirName);
                
                ret = true;
            }
            catch (Exception error)
            {
                _logger.LogError(error,"{Source}: {Message}", error.Source, error.Message);
                
                ret = false;
            }
        }
        
        return ret;
    }
    
    public async Task<bool> Initialize(CancellationToken cancel = default)
    {
        var ret = true;
        
        var timer = Stopwatch.StartNew();

        byte errorsCounter = 0;
        
        _logger.LogInformation("Start database initializing");
        
        if (Convert.ToBoolean(_configuration["Database:ReCreateOnStartup"]))
        {
            _logger.LogInformation("Recreate database");
            
            try
            {
                await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);
                
                if (RestoreResourcesDirectory())
                    await _db.Database.EnsureCreatedAsync(cancel).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                _logger.LogError(error,"{Source}: {Message}", error.Source, error.Message);
                ret = false;
                errorsCounter++;
            }
            
            _logger.LogInformation("Recreate database finished. {time} ms | {errorCount} errors",
                timer.ElapsedMilliseconds,errorsCounter);

            errorsCounter = 0;
        }

        _logger.LogInformation("Apply migration onto database");
        try
        {
            await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);
        }
        catch (Exception  error)
        {
            _logger.LogError(error,"{Source}: {Message}", error.Source, error.Message);
            //Todo: find best way handling sql exceptions
            //ret = false;
            errorsCounter++;
        }
        
        _logger.LogInformation("Apply migration finished. {time} ms | {errorCount} errors",
            timer.ElapsedMilliseconds,errorsCounter);
        
        _logger.LogInformation("Database initializing finished {time} ms", timer.Elapsed.TotalMilliseconds);

        return ret;
    }

    #endregion
}