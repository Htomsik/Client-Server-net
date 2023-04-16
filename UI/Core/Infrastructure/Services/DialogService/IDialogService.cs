using System.Reactive;
using System.Windows.Input;
using Core.Infrastructure.VMD.Interfaces;
using ReactiveUI;

namespace Core.Infrastructure.Services.DialogService;

public interface IViewDialogService : IReactiveObject
{
    public bool IsOpen { get;}
    
    public IDialogVmd? DialogVmd { get; }
    
}

public interface IVmdDialogService : IViewDialogService
{
    public void ChangeVmd(Type dialogVmdType);
    
    public void ChangeVmdAndOpen(Type dialogVmdType);
}

public interface IDialogService : IVmdDialogService
{
    
    public IReactiveCommand Open { get; }

    public ReactiveCommand<Unit,Unit> Cancel { get; }

    public ICommand Process { get; }
}