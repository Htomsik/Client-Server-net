namespace Core.Infrastructure.Services.EncryptService;

public interface IDecryptService
{
    public string Decrypt(string enCryptText);
    
    public string Decrypt(byte[] enCryptBytes);
}