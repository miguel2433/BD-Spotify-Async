using MySqlConnector;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySQL");

// Registrar IDbConnection
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

// MVC
builder.Services.AddControllersWithViews();


// Repos
builder.Services.AddScoped<IRepoCancionAsync, RepoCancionAsync>();
builder.Services.AddScoped<IRepoArtistaAsync, RepoArtistaAsync>();
builder.Services.AddScoped<IRepoAlbumAsync, RepoAlbumAsync>();
builder.Services.AddScoped<IRepoGeneroAsync, RepoGeneroAsync>();
builder.Services.AddScoped<IRepoUsuarioAsync, RepoUsuarioAsync>();
builder.Services.AddScoped<IRepoNacionalidadAsync, RepoNacionalidadAsync>();
builder.Services.AddScoped<IRepoPlaylistAsync,RepoPlaylistAsync>();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Usuario/Login";
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();


// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();   
app.UseAuthorization();    // Debe ir después de Authentication

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
