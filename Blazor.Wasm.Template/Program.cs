using Blazor.Wasm.Template;
using Blazor.Wasm.Template.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("Spoonacular", client =>
{
    client.BaseAddress = new Uri("https://api.spoonacular.com");
});
builder.Services.AddScoped<IRecipesService,RecipesService>();

await builder.Build().RunAsync();
