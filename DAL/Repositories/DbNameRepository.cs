using DAL.Context;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace DAL.Repositories;

public class DbNameRepository<T> : DbRepository<T>, INamedRepository<T> where T : NamedEntity, new()
{
    #region Constructos
    public DbNameRepository(DataDb db) : base(db) { }
    #endregion

    #region Methods
    #region Item interactions
    public async Task<T?> Get(string name, CancellationToken cancel = default) =>
        await Items.FirstOrDefaultAsync(elem => elem.Name == name, cancel).ConfigureAwait(false);

    public async Task<bool> Delete(string name, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Name == name);
        if (item is null)
            item = await Set
                .Select(i => new T { Id = i.Id, Name = i.Name })
                .FirstOrDefaultAsync(i => i.Name == name, cancel)
                .ConfigureAwait(false);
        if (item is null)
            return false;

        return await Delete(item, cancel).ConfigureAwait(false);
    }
    #endregion

    #region Extensios

    public async Task<bool> Exist(string name, CancellationToken cancel = default) =>
        await Items.AnyAsync(elem => elem.Name == name, cancel).ConfigureAwait(false);

    #endregion
    #endregion
}