using Core.Infrastructure.Models.Other;

namespace Core.Infrastructure.Services.Other;

public interface INotificationService
{
    public TimeSpan TimeOut { get; set; }
    
    public void Notify(string title,string message = "", NotifyLevel level = NotifyLevel.Information);
}