using BlazorWebApp.Components;
using BlazorWebApp.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeatherApi", builder =>
    {
        builder.WithOrigins("https://localhost:7266") // Replace with your actual API URL if different
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Enable if your app uses authentication cookies
    });
});

// Configure HttpClient for WeatherApiClient
builder.Services.AddHttpClient<WeatherApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7266"); // API base address
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
