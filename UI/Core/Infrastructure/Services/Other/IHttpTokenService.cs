namespace Core.Infrastructure.Services.Other;

public interface IHttpTokenService
{
    public bool SetToken(HttpClient client);
}