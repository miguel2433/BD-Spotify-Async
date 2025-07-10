using System.Data;
using MySqlConnector;
using Scalar.AspNetCore;
using Spotify.Core;
using Spotify.ReposDapper;
using Spotify.Core.Persistencia;

var builder = WebApplication.CreateBuilder(args);

//  Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MySQL");

//  Registrando IDbConnection para que se inyecte como dependencia
//  Cada vez que se inyecte, se creará una nueva instancia con la cadena de conexión
builder.Services.AddScoped<IDbConnection>(sp => new MySqlConnection(connectionString));

//Cada vez que necesite la interfaz, se va a instanciar automaticamente AdoDapper y se va a pasar al metodo de la API
builder.Services.AddScoped<IRepoGeneroAsync, RepoGeneroAsync>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

//Para un GET en la ruta "/todoitems", 
app.MapGet("/genero", async (IRepoGeneroAsync repo) => 
    await repo.Obtener());

app.MapGet("/genero/{id}", async (IRepoGeneroAsync repo, byte id) => await repo.DetalleDe(id));

app.MapPost("/genero", async (IRepoGeneroAsync repo, Genero genero) => await repo.Alta(genero));
app.Run();