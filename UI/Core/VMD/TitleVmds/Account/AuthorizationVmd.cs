using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Interfaces.Entities;
using ReactiveUI;

namespace Core.VMD.TitleVmds.Account;

public class AuthorizationVmd : BaseTitleVmd, IDialogVmd
{
    #region Properties

    public override string Title { get; } = "Authorization";
    
    public  IUser Account { get; set; }

    #endregion

    #region Fields

    protected readonly IAccountService AccountService;

    #endregion
    
    #region Constructors

    public AuthorizationVmd(IAccountService accountService, IStore<User> userStore)
    {
        AccountService = accountService;
        
        Account = userStore.CurrentValue;

        userStore.CurrentValueChangedNotifier += () => Account = userStore.CurrentValue;
        
    }
    
    #endregion
    
    #region Methods

    public virtual async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Authorization(cancel);
    
    #endregion
    
}