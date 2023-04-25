using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Services.DialogService;
using Core.Infrastructure.Stores.Interfaces;
using Core.VMD.TitleVmds;
using ReactiveUI;

namespace Core.VMD.DevPanelVmds;

public sealed class AccountDevVmd : AccountVmd
{
    #region Constructors

    public AccountDevVmd(
        ISaverStore<User, bool> userStore, 
        IVmdDialogService dialogService, 
        ITokenService tokenService) : base(userStore, dialogService)
    {
        RefreshTokens = ReactiveCommand.CreateFromTask(tokenService.Refresh,CanRefreshTokens);
    }

    #endregion
    
    #region Commands

    public IReactiveCommand RefreshTokens { get; }
    
    private IObservable<bool> CanRefreshTokens =>
        this.WhenAnyValue(
            x => x.Account.Tokens.RefreshToken,
            rtoken=>!string.IsNullOrEmpty(rtoken));

    #endregion
}
   