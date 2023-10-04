using Core.Infrastructure.Models.Other;

namespace Core.Infrastructure.Services.Other;

public interface IUiThemeService
{
    public void ChangeMode(ThemeMode mode);
}