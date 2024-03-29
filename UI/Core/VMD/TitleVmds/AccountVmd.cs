﻿using Core.Infrastructure.Models.Entities;
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
    
    #endregion

    #region Fields

    private readonly IVmdDialogService _dialogService;

    #endregion
    
    #region Constructors

    public AccountVmd(
        ISaverStore<User, bool> userStore,
        IVmdDialogService dialogService)
    {
        _dialogService = dialogService;
        
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

        Deactivate = ReactiveCommand.Create(()=>_dialogService.ChangeVmdAndOpen(typeof(DeactivateAccountVmd)), CanLogout);

        Login = ReactiveCommand.Create(()=> _dialogService.ChangeVmdAndOpen(typeof(AuthorizationVmd)),CanLogin);
        
        Registration = ReactiveCommand.Create(()=> _dialogService.ChangeVmdAndOpen(typeof(RegistrationVmd)),CanLogin);
        
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

    #region Deactivate

    public IReactiveCommand Deactivate { get; }

    #endregion
    
    #endregion
}