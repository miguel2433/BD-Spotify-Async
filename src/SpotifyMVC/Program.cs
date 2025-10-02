using MySqlConnector;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using System.Data;


var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("MySQL");

// Registrar IDbConnection para inyección de dependencias
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepoCancionAsync, RepoCancionAsync>();
builder.Services.AddScoped<IRepoArtistaAsync, RepoArtistaAsync>();
builder.Services.AddScoped<IRepoAlbumAsync, RepoAlbumAsync>();
builder.Services.AddScoped<IRepoGeneroAsync, RepoGeneroAsync>();

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