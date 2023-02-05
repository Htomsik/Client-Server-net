using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorUI;
using BlazorUI.Infrastructure.IOC;
using Domain.Entities;
using HTTPClients.Repositories;
using Interfaces.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddApi<IRepository<DataSourceInfo>, HttpRepository<DataSourceInfo>>("api/SourceRepository/");

await builder.Build().RunAsync();



