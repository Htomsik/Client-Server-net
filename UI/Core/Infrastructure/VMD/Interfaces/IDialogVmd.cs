namespace Core.Infrastructure.VMD.Interfaces;

public interface IDialogVmd : ITitleVmd
{
    public Task<bool> Process(CancellationToken cancel = default);

    public IObservable<bool> CanProses { get; }
}