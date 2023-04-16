using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Models.Settings;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Services.DialogService;
using Core.Infrastructure.Stores.Interfaces;
using Core.Infrastructure.VMD;
using Core.VMD.TitleVmds.Account;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public class AccountVmd : BaseTitleVmd
{
    #region Properties

    public override string Title { get; } = "Account";

    [Reactive]
    public User Account { get; set; }
    
    [Reactive]
    public Settings? Settings { get; set; }

    #endregion

    #region Fields

    private readonly IVmdDialogService _dialogService;

    #endregion
    
    #region Constructors

    public AccountVmd(
        ISaverStore<User, bool> userStore,
        IStore<Settings> settingsStore,
        IVmdDialogService dialogService,
        ITokenService tokenService)
    {
        _dialogService = dialogService;
        
        Account = userStore.CurrentValue;
        
        Settings = settingsStore.CurrentValue;

        #region Subscriptions

        userStore.CurrentValueChangedNotifier += _ =>
        {
            Account = userStore.CurrentValue;
        };

        settingsStore.CurrentValueChangedNotifier += () =>
        {
            Settings = settingsStore.CurrentValue;
        };
        
        #endregion

        #region Commands

        Logout = ReactiveCommand.Create(()=>
        {
            userStore.CurrentValue = new User();
            userStore.SaveNow();
        },CanLogout);

        Login = ReactiveCommand.Create(()=> _dialogService.ChangeVmdAndOpen(typeof(AuthorizationVmd)),CanLogin);
        
        Registration = ReactiveCommand.Create(()=> _dialogService.ChangeVmdAndOpen(typeof(RegistrationVmd)),CanLogin);

        RefreshTokens = ReactiveCommand.CreateFromTask(tokenService.Refresh,CanRefreshTokens);

        #endregion
    }

    #endregion

    #region Commands

    #region Login

    public IReactiveCommand Login { get;}

    private IObservable<bool> CanLogin =>
        this.WhenAnyValue(x => x.Account.Tokens.Token,string.IsNullOrEmpty);

    #endregion

    #region Registration

    public IReactiveCommand Registration { get; }

    #endregion
    
    #region RefreshTokens

    public IReactiveCommand RefreshTokens { get; }
    
    private IObservable<bool> CanRefreshTokens =>
        this.WhenAnyValue(
            x => x.Account.Tokens.RefreshToken,
            rtoken=>!string.IsNullOrEmpty(rtoken));

    #endregion

    #region Logout
    public IReactiveCommand Logout { get;}

    private IObservable<bool> CanLogout =>
        this.WhenAnyValue(x => x.Account.Name, acc=> !string.IsNullOrEmpty(acc));

    #endregion
    
    #endregion
}