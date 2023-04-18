using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using static System.Security.Cryptography.ProtectedData;

namespace Core.Infrastructure.Services.EncryptService;

internal sealed class Base64EncryptService : IEncryptService
{
    #region Fields

    private readonly ILogger _logger;

    #endregion

    #region Constants

    // Todo learn how to properly store entropy
    private static readonly  byte[] Entropy = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    #endregion

    #region Constructors

    public Base64EncryptService(ILogger<Base64EncryptService> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Methods

    public string Encrypt(string deCryptText)
    {
        if (string.IsNullOrEmpty(deCryptText))
        {
            _logger.LogError("{Text} can't be null", nameof(deCryptText));
            return string.Empty;
        }

        return Encrypt(Encoding.Unicode.GetBytes(deCryptText));
    }

    public string Encrypt(byte[] deCryptBytes)
    {
        if (deCryptBytes.Length == 0)
        {
            _logger.LogError("{Text} can't be null", nameof(deCryptBytes));
            return string.Empty;
        }
        var enCryptText = Protect(deCryptBytes, Entropy, DataProtectionScope.CurrentUser);

        return Convert.ToBase64String(enCryptText);
    }

    public string Decrypt(string enCryptText)
    {
        if (string.IsNullOrEmpty(enCryptText))
        {
            _logger.LogError("{Text} can't be null", nameof(enCryptText));
            return string.Empty;
        }

        return Decrypt(Convert.FromBase64String(enCryptText));
    }

    public string Decrypt(byte[] enCryptBytes)
    {
        if (enCryptBytes.Length == 0)
        {
            _logger.LogError("{Text} can't be null", nameof(enCryptBytes));
            return string.Empty;
        }

        var deCryptText = Unprotect(enCryptBytes, Entropy, DataProtectionScope.CurrentUser);
        
        return Encoding.Unicode.GetString(deCryptText);
    }

    #endregion
    
}