using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Core.VMD.TitleVmds.Account;

public sealed class DeactivateAccountVmd : BaseAccountOperationVmd<AuthUser>
{
    public override string Title { get; } = "Deactivate Account";

    [Reactive] public bool DelConfirmation { get; set; } = false;

    private IObservable<bool> DelConfirmationObservable =>
        this.WhenAnyValue(x => x.DelConfirmation);

    private IObservable<bool> _accountIsValidObservable;

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

    protected override void InitProcess()
    {
        _accountIsValidObservable = Account.IsValid();
        
        CanProcess =
            this.WhenAnyObservable(
                x => x.DelConfirmationObservable,
                x => x._accountIsValidObservable,
                ((b, b1) => b && b1)
            );
    }
}