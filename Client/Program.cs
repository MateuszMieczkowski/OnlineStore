using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnlineStore.Client;
using OnlineStore.Client.Brokers.API;
using OnlineStore.Client.Configurations;
using OnlineStore.Client.Providers;
using OnlineStore.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<ToastService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
    p.GetRequiredService<ApiAuthenticationStateProvider>());

builder.Services.AddScoped<IApiBroker, ApiBroker>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var url = builder.Configuration.Get<Configuration>().BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });
await builder.Build().RunAsync();