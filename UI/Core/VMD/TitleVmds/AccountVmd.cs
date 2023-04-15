using System.Reactive.Linq;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.Stores.Interfaces;
using Core.Infrastructure.VMD;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.VMD.TitleVmds;

public class AccountVmd : BaseTitleVmd
{
    #region Properties

    public override string Title { get; } = "Account";

    [Reactive]
    public User Account { get; set; }

    #endregion
    
    #region Constructors

    public AccountVmd(ISaverStore<User, bool> userStore, IAccountService accountService)
    {
        Account = userStore.CurrentValue;

        #region Subscriptions

        userStore.CurrentValueChangedNotifier += _ =>
        {
            Account = userStore.CurrentValue;
        };

        #endregion

        #region Commands

        Logout = ReactiveCommand.Create(()=>
        {
            userStore.CurrentValue = new User();
            userStore.SaveNow();
        },CanLogout);

        Login = ReactiveCommand.CreateFromObservable((() => 
            Observable
            .StartAsync(accountService.Authorization)), CanLogin);
        

        Registration = ReactiveCommand.CreateFromTask(accountService.Registration,CanLogin);

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

    #region Logout
    public IReactiveCommand Logout { get;}

    private IObservable<bool> CanLogout =>
        this.WhenAnyValue(x => x.Account.Name, acc=> !string.IsNullOrEmpty(acc));

    #endregion

    
    #endregion
}