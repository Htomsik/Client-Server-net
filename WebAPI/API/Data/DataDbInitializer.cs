using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

internal sealed class  DataDbInitializer : IDbInitializer
{
    #region Fields

    private readonly DataDb _db;

    private readonly IConfiguration _configuration;

    #endregion

    #region Constructors

    public DataDbInitializer(DataDb db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public async Task<bool> Initialize(CancellationToken cancel = default)
    {
        if (Convert.ToBoolean(_configuration["Database:ReCreateOnStartup"]))
        {
            await _db.Database.EnsureDeletedAsync(cancel).ConfigureAwait(false);

            await _db.Database.EnsureCreatedAsync(cancel).ConfigureAwait(false);
        }
        
        await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);

        return await _db.Sources.AnyAsync(cancel).ConfigureAwait(false);
    }

    #endregion
}