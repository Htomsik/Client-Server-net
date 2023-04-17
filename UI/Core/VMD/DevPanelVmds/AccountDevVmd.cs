using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Services.DialogService;
using Core.Infrastructure.Stores.Interfaces;
using Core.VMD.TitleVmds;
using ReactiveUI;

namespace Core.VMD.DevPanelVmds;

public sealed class AccountDevVmd : AccountVmd
{
    public AccountDevVmd(
        ISaverStore<User, bool> userStore, 
        IStore<Settings> settingsStore, 
        IVmdDialogService dialogService, 
        ITokenService tokenService) 
        : base(userStore, settingsStore, dialogService, tokenService)
    {
        RefreshTokens = ReactiveCommand.CreateFromTask(tokenService.Refresh,CanRefreshTokens);
    }
    
    #region Commands

    public IReactiveCommand RefreshTokens { get; }
    
    private IObservable<bool> CanRefreshTokens =>
        this.WhenAnyValue(
            x => x.Account.Tokens.RefreshToken,
            rtoken=>!string.IsNullOrEmpty(rtoken));

    #endregion
}
   