using Interfaces.Entities;

namespace Interfaces.Repositories;

public interface IRepository<T> where T : IEntity
{
    #region Collections interactions

    Task<IEnumerable<T>> GetMany(CancellationToken cancel = default);

    Task<IEnumerable<T>> GetMany(int skip, int count, CancellationToken cancel = default);
    
    #endregion

    #region Item interactions

    Task<T?> Get(int id, CancellationToken cancel = default);
    
    Task<IPageItem<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default);
    
    Task<bool> Add(T item, CancellationToken cancel = default);

    Task<bool> Update(T item, CancellationToken cancel = default);

    Task<bool> Delete(T item, CancellationToken cancel = default);
    
    Task<bool> Delete(int id, CancellationToken cancel = default);
    
    #endregion

    #region Extensions

    Task<int> Count(CancellationToken cancel = default);

    Task<bool> Exist(int id, CancellationToken cancel = default);

    Task<bool> Exist(T item, CancellationToken cancel = default);

    #endregion
}