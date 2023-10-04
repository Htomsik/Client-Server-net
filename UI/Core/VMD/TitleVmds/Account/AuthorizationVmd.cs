using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;

namespace Core.VMD.TitleVmds.Account;

public sealed class AuthorizationVmd : BaseAccountOperationVmd<AuthUser> 
{
    #region Properties

    public override string Title { get; } = "Authorization";
    
    #endregion
    
    #region Constructors

    public AuthorizationVmd(IAccountService<AuthUser,RegUser> accountService) : base(accountService) { }
    
    #endregion
    
    #region Methods

    public override async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Authorization(Account, cancel);
    
    protected override void InitAccount() => Account = new AuthUser(true);
    
    #endregion
}