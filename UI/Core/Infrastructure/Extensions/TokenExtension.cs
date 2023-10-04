using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.EncryptService;
using Interfaces.Other;

namespace Core.Infrastructure.Extensions;

internal static class TokenExtension
{
    public static ITokens Decrypt(this ITokens tokens, IDecryptService decryptService)
    {
        return new Tokens
        {
            Token = decryptService.Decrypt(tokens.Token),
            RefreshToken = decryptService.Decrypt(tokens.RefreshToken),
        };
    }
}