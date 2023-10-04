using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;

namespace Core.VMD.TitleVmds.Account;

public sealed class RegistrationVmd : BaseAccountOperationVmd<RegUser>
{
    public override string Title { get; } = "Registration";

    public RegistrationVmd(IAccountService<AuthUser, RegUser> accountService) : base(accountService) { }

    #region Methods

    public override async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Registration(Account,cancel);
    
    protected override void InitAccount() => Account = new RegUser(true);
    
    #endregion
}