namespace API.Data;

public interface IDbInitializer
{
    Task<bool> Initialize(CancellationToken cancel = default);
}