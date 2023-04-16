using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Core.Infrastructure.Hosting;
using Core.Infrastructure.VMD;
using Core.Infrastructure.VMD.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Core.Infrastructure.Services.DialogService;

public class DialogService : BaseVmd, IDialogService
{
    #region Properties

    [Reactive]
    public bool IsOpen { get; private set; }
    
    [Reactive]
    public IDialogVmd? DialogVmd { get; private set; }

    #endregion
    
    #region Constructors

    public DialogService()
    {
        #region Commands initialize

        Cancel = ReactiveCommand.Create(() => { IsOpen = false; });
        
        Open = ReactiveCommand.Create(() => { IsOpen = true;},CanOpen);
        
        #endregion
    }

    #endregion

    #region Commands

    #region Open

    public IReactiveCommand Open { get; }

    private IObservable<bool> CanOpen =>
        this.WhenAnyValue(
            x => x.DialogVmd,
            x => x.IsOpen,
            (vmd, open) => vmd != null && !open);
    
    #endregion
    
    #region Process
    
    public ICommand Process { get; private set; }
    
    #endregion
    
    public ReactiveCommand<Unit,Unit> Cancel { get; }
    
    #endregion

    #region Methods

    public void ChangeVmd(Type dialogVmdType)
    {
        if (dialogVmdType.GetInterface(nameof(IDialogVmd)) is null)
            throw new ArgumentException($"{nameof(DialogVmd)} must implement {nameof(IDialogVmd)}");
        
        if (HostWorker.Services.GetRequiredService(dialogVmdType) is {} newDialog)
            DialogVmd = (IDialogVmd?)newDialog;
    }

    public void ChangeVmdAndOpen(Type dialogVmdType)
    {
        ChangeVmd(dialogVmdType);

        if (DialogVmd != null)
        {
            Process =
                ReactiveCommand
                    .CreateFromObservable(() => 
                        Observable.StartAsync(ProcessAndClose)
                            .TakeUntil(Cancel),DialogVmd?.CanProcess);
            
            IsOpen = true;
        }
            
    }

    private async Task ProcessAndClose(CancellationToken cancel)
    {
        var result = await DialogVmd.Process(cancel);

        if (result)
            IsOpen = false;
    }

    #endregion
}