using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

internal sealed class  DataDbInitializer : IDbInitializer
{
    #region Fields

    private readonly DataDb _db;

    #endregion

    #region Constructors

    public DataDbInitializer(DataDb db) => _db = db;

    #endregion

    #region Methods

    public async Task<bool> Initialize(CancellationToken cancel = default)
    {
        await _db.Database.MigrateAsync(cancel).ConfigureAwait(false);

        if (await _db.Sources.AnyAsync(cancel).ConfigureAwait(false)) return true;

        return false;
    }

    #endregion
}