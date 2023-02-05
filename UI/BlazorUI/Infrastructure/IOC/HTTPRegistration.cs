using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorUI.Infrastructure.IOC;

internal static partial class IocRegistration
{
    public static IHttpClientBuilder AddApi<TInterface, TClient>(this IServiceCollection services, string address)
        where TInterface : class
        where  TClient : class,
        TInterface =>
        services.AddHttpClient<TInterface,TClient>((host, client) =>
        {
            client.BaseAddress = new Uri($"{host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress}{address}" );
        });
}