using Interfaces.Entities;

namespace Interfaces.Repositories;

public interface IRepository<T> where T : IEntity
{
    #region Collections interactions

    Task<IEnumerable<T>> GetMany(CancellationToken cancel = default);

    Task<IEnumerable<T>> GetMany(int skip, int count, CancellationToken cancel = default);
    
    #endregion

    #region Item interactions

    Task<T> Get(int id, CancellationToken cancel = default);
    
    Task<T> Add(T item, CancellationToken cancel = default);

    Task<T> Update(T item, CancellationToken cancel = default);

    Task<T> Delete(T item, CancellationToken cancel = default);
    
    #endregion

    #region Extensions

    Task<int> Count(CancellationToken cancel = default);

    Task<bool> Exist(int id, CancellationToken cancel = default);

    Task<bool> Exist(T item, CancellationToken cancel = default);

    #endregion
}