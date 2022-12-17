using System.Collections.ObjectModel;
using AppInfrastructure.Stores.Repositories.Collection;
using Core.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;

namespace Core.Infrastructure.IOC;

public partial class IocRegistrator
{
    public static IServiceCollection StoresRegistrator(this IServiceCollection services) =>
        services
            .AddSingleton<ICollectionRepository<ObservableCollection<LogEvent>,LogEvent>, LogStore>();
}