
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Interfaces.Entities;

namespace Core.VMD.TitleVmds.Account;

public class AuthorizationVmd : BaseTitleVmd, IDialogVmd
{
    #region Properties

    public override string Title { get; } = "Authorization";

    public IAuthUser Account { get; set; } = new AuthUser();

    #endregion

    #region Fields

    protected readonly IAccountService<AuthUser> AccountService;

    #endregion
    
    #region Constructors

    public AuthorizationVmd(IAccountService<AuthUser> accountService)
    {
        AccountService = accountService;
    }
    
    #endregion
    
    #region Methods

    public virtual async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Authorization((AuthUser)Account, cancel);
    
    #endregion
    
}