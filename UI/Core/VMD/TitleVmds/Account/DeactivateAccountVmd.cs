using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;

namespace Core.VMD.TitleVmds.Account;

public sealed class DeactivateAccountVmd : BaseAccountOperationVmd<AuthUser>
{
    public override string Title { get; } = "Deactivate Account";

    public DeactivateAccountVmd(IAccountService<AuthUser, RegUser> accountService) : base(accountService)
    { }

    public override async Task<bool> Process(CancellationToken cancel = default)
    { 
        return await AccountService.Deactivate(cancel);
    }

    protected override void InitAccount()
    {
        Account = new AuthUser();
    }
}