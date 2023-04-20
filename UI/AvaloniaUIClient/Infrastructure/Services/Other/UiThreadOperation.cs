using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using Core.Infrastructure.Services.Other;

namespace AvaloniaUIClient.Infrastructure.Services.Other;

internal sealed class UiThreadOperation : IUiThreadOperation
{
    public async Task InvokeAsync(Action operation) =>  await Dispatcher.UIThread.InvokeAsync(operation.Invoke);
}