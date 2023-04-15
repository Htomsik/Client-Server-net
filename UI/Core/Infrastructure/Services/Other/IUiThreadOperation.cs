namespace Core.Infrastructure.Services.Other;

public interface IUiThreadOperation
{
    public Task InvokeAsync(Action operation);
}