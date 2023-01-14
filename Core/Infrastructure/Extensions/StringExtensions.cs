using System.Runtime.CompilerServices;

namespace Core.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string MessageTemplateBuilder(string? message = null,[CallerMemberName] string? method = null) =>  $"{nameof(FileExtension)}/{method}: {message}";
}