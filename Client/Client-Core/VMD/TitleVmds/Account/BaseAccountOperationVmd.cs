using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Interfaces.Entities;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Core.VMD.TitleVmds.Account;

public abstract class BaseAccountOperationVmd<TAccount> : BaseTitleVmd, IDialogVmd
where TAccount : ReactiveValidationObject, IAuthUser, new()
{
    #region Properties

    public TAccount Account { get; protected set; } 

    #endregion

    #region Fields

    protected readonly IAccountService<AuthUser,RegUser> AccountService;

    #endregion
    
    #region Constructors

    public BaseAccountOperationVmd(IAccountService<AuthUser,RegUser> accountService)
    {
        AccountService = accountService;

        InitAccount();
        
        CanProcess = Account.IsValid();
    }

    #endregion

    #region Methods

    public abstract Task<bool> Process(CancellationToken cancel = default);
    
    public IObservable<bool> CanProcess { get; protected set; }

    protected abstract void InitAccount();

    #endregion
}