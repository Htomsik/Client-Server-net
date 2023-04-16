using AppInfrastructure.Stores.DefaultStore;
using Core.Infrastructure.Models.Entities;
using Core.Infrastructure.Services.AccountService;

namespace Core.VMD.TitleVmds.Account;

public sealed class RegistrationVmd : AuthorizationVmd
{
    public override string Title { get; } = "Registration";
    
    public RegistrationVmd(IAccountService accountService, IStore<User> userStore) : base(accountService, userStore) { }

    #region Methods

    public override async Task<bool> Process(CancellationToken cancel = default) => await AccountService.Registration(cancel);
    
    #endregion
    
}