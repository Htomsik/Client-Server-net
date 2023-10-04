using Core.Infrastructure.Models.Other;

namespace Core.Infrastructure.Services.Other;

public interface INotificationService
{
    public TimeSpan TimeOut { get; set; }
    
    public void Notify(string message,string title = "", NotifyLevel level = NotifyLevel.Information);
}