namespace Interfaces.Repositories;

public interface ITemPage<T>
{
    IEnumerable<T> Items { get; }
    
    int TotalCount { get; }
    
    int PageIndex { get; }
    
    int PageSize { get; }
    
    int TotalPagesCount { get; }
}