using DAL.Context;
using Interfaces.Entities;
using Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DbRepository<T> : IRepository<T> where T : class, IEntity, new()
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
    protected record PageItem(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPageItem<T>
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
    public async Task<IPageItem<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
    {
        if (pageSize <= 0)
            return new PageItem(Enumerable.Empty<T>(), pageSize, pageIndex, pageSize);

        var query = Items;
        
        var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);

        if (totalCount == 0)
            return new PageItem(Enumerable.Empty<T>(), 0, pageIndex, pageSize);
        
        if (query is not IOrderedQueryable<T>)
            query = query.OrderBy(item => item.Id);

        if (pageIndex > 1)
            query = query.Skip(pageIndex * pageSize);

        query = query.Take(pageSize);

        var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

        return new PageItem(items, totalCount, pageIndex, pageSize);
    }
    #endregion
    
    #region Collections interactions
    public async Task<IEnumerable<T>> GetMany(CancellationToken cancel = default) =>
        await Items.ToArrayAsync(cancel).ConfigureAwait(false);
    
    public async Task<IEnumerable<T>> GetMany(int skip, int count, CancellationToken cancel = default)
    {
        if (count <= 0) 
            return Enumerable.Empty<T>();
        
        IQueryable<T> query = Items switch
        {
            IOrderedQueryable<T> orderedQuery => orderedQuery,
            { } q => q.OrderBy(i => i.Id)
        };

        if (skip > 0)
            query = query.Skip(skip);
        
        return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
    }
    #endregion

    #region Item interactions
    public  async Task<T?> Get(int id, CancellationToken cancel = default)
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
    
    public async Task<T?> Add(T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        
        if (!await ValidateItem(item, cancel))
            return null;
        
        await _dataDb.AddAsync(item, cancel).ConfigureAwait(false);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }
    
    public async Task<T?> Update(T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        _dataDb.Update(item);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }

    public async Task<T?> Delete(T item, CancellationToken cancel = default)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));
        
        _dataDb.Remove(item);
        
        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }

    public async Task<T?> Delete(int id, CancellationToken cancel = default)
    {
        var elemToDelete = Set.Local.FirstOrDefault(elem => elem.Id == id);

        elemToDelete ??=
            await Set
                .Select(i => new T { Id = i.Id })
                .FirstOrDefaultAsync(elem => elem.Id == id, cancel)
                .ConfigureAwait(false);

        if (elemToDelete is null)
            return null;
        
        return await Delete(elemToDelete,cancel).ConfigureAwait(false);
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

    protected virtual async Task<bool> ValidateItem(T item, CancellationToken cancel = default) => await Exist(item, cancel);

    #endregion

    #endregion
}