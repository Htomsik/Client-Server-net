using Interfaces.Entities;

namespace Interfaces.Repositories;

public interface INamedRepository<T> : IRepository<T> where T:INamedEntity
{
    #region Item Interactions
    Task<T> Get(string name, CancellationToken cancel = default);
    #endregion

    #region Extensions
    Task<T> Exist(string name, CancellationToken cancel = default);
    #endregion
}