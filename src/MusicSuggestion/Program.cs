using MusicSuggestion.Abstractions.Options;
using MusicSuggestion.SpotifyApi.Interfaces;
using MusicSuggestion.SpotifyApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SpotifyOptions>(
    builder.Configuration.GetSection("Spotify"));
builder.Services.AddSingleton<ISpotifyClient, SpotifyClient>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ISpotifyCacheService, SpotifyCacheService>();
builder.Services.AddTransient<ISpotifyService, SpotifyService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
