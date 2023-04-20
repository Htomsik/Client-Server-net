namespace Core.Infrastructure.Services.EncryptService;

public interface IEncryptService : IDecryptService
{
    public string Encrypt(string deCryptText);
    
    public string Encrypt(byte[] deCryptBytes);
}

