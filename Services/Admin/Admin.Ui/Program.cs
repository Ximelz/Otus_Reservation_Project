using Admin.Ui.Components;
using Admin.Ui.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpContextAccessor нужен для работы с cookies
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<AdminApiOptions>(builder.Configuration.GetSection("AdminApi"));
// Scoped сервис использует cookies для сохранения токена между навигациями
builder.Services.AddScoped<AdminApiState>();
builder.Services.AddHttpClient<AdminApiClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<AdminApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
