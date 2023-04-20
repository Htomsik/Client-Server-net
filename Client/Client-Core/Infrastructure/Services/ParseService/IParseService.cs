namespace Core.Infrastructure.Services.ParseService;

public interface IParseService
{
    public string Serialize(object nonSerialized);

    public T? DeSerialize<T>(string serialized);
}