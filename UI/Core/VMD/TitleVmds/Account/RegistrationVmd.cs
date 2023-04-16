using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;

namespace Core.VMD.TitleVmds.Account;

public sealed class RegistrationVmd : AuthorizationVmd
{
    public override string Title { get; } = "Registration";
    
    public RegistrationVmd(IAccountService<AuthUser> accountService) : base(accountService) { }

    #region Methods

    public override async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Registration((AuthUser)Account,cancel);
    
    #endregion
    
}