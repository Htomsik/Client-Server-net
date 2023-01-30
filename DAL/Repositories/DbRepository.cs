using DAL.Context;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace DAL.Repositories;

public class DbRepository<T> : IRepository<T> where T : Entity, new()
{
    #region Properties
    protected  IQueryable<T> Items => Set;
    
    public bool AutoSaveChanges { get; set; } = true;
    #endregion
    
    #region Fields
    private readonly DataDb _dataDb;
    
    protected readonly DbSet<T> Set;
    #endregion

    #region Records
    protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : ITemPage<T>
    {
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
    #endregion

    #region Constructors
    public DbRepository(DataDb db)
    {
        _dataDb = db;
        Set = db.Set<T>();
    }
    #endregion

    #region Methods
    #region Page interactions
    public async Task<ITemPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
    {
        if (pageSize <= 0)
            return new Page(Enumerable.Empty<T>(), pageSize, pageIndex, pageSize);

        var query = Items;
        
        var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);

        if (totalCount == 0)
            return new Page(Enumerable.Empty<T>(), 0, pageIndex, pageSize);

        if (pageIndex > 0)
            query = query.Skip(pageIndex * pageSize);

        query = query.Take(pageSize);

        var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

        return new Page(items, totalCount, pageIndex, pageSize);
    }
    #endregion
    
    #region Collections interactions
    public async Task<IEnumerable<T>> GetMany(CancellationToken cancel = default) =>
        await Items.ToArrayAsync(cancel).ConfigureAwait(false);
    
    public async Task<IEnumerable<T>> GetMany(int skip, int count, CancellationToken cancel = default)
    {
        if (count <= 0) 
            return Enumerable.Empty<T>();

        var query = Items;

        if (skip > 0)
            query = query.Skip(skip);
        
        return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
    }
    #endregion

    #region Item interactions
    public async Task<T?> Get(int id, CancellationToken cancel = default)
    {
        switch(Items)
        {
            case DbSet<T> set :
                return await set.FindAsync(new object[] { id }, cancel).ConfigureAwait(false);
            case { } items:
                return await items.FirstOrDefaultAsync(elem => elem.Id == id, cancel).ConfigureAwait(false);
            default:
                throw new InvalidDataException("Undefined data source");
        }
    }
    
    public async Task<bool> Add(T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        
        await _dataDb.AddAsync(item, cancel).ConfigureAwait(false);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return true;
    }
    
    public async Task<bool> Update( T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        _dataDb.Update(item);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> Delete(T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        
        if (await Exist(item, cancel))
            return false;

        _dataDb.Remove(item);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> Delete(int id, CancellationToken cancel = default)
    {
        var elemToDelete = Set.Local.FirstOrDefault(elem => elem.Id == id);

        elemToDelete ??=
            await Set
                .Select(i => new T { Id = i.Id })
                .FirstOrDefaultAsync(elem => elem.Id == id, cancel)
                .ConfigureAwait(false);

        if (elemToDelete is null)
            return false;
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return true;
    }
    #endregion

    #region Extensions
    public async Task<int> Count(CancellationToken cancel = default) =>
        await Items.CountAsync(cancel).ConfigureAwait(false);
    
    public async Task<bool> Exist(int id, CancellationToken cancel = default) =>
        await Items.AnyAsync(elem => elem.Id == id, cancel).ConfigureAwait(false);

    public async Task<bool> Exist(T item, CancellationToken cancel = default) =>
        item is null ?
            throw new ArgumentNullException(nameof(item)) :
            await Items.AnyAsync(elem=>elem.Id == item.Id,cancel).ConfigureAwait(false);
    
    public async Task<int> SaveChanges(CancellationToken cancel = default) => 
        await _dataDb.SaveChangesAsync(cancel).ConfigureAwait(false);

    #endregion
    #endregion
}