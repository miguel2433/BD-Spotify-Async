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
builder.Services.AddScoped<IRepoNacionalidadAsync, RepoNacionalidadAsync>();
builder.Services.AddScoped<IRepoAlbumAsync, RepoAlbumAsync>();

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


app.MapGet("/nacionalidad", async (IRepoNacionalidadAsync repo) => 
    await repo.Obtener()
        is List<Nacionalidad> ListaNacionalidades
            ? Results.Ok(ListaNacionalidades)
            : Results.NotFound());

app.MapGet("/nacionalidad/{id}", async (IRepoNacionalidadAsync repo, uint id) => 
    await repo.DetalleDe(id)
        is Nacionalidad nacionalidad 
            ? Results.Ok(nacionalidad)
            : Results.NotFound()

);

app.MapPost("/nacionalidad", async (IRepoNacionalidadAsync repo, Nacionalidad nacionalidad) => 
{  
    await repo.Alta(nacionalidad);

    return Results.Created($"/genero/{nacionalidad.idNacionalidad}", nacionalidad);   
});

app.MapGet("/genero", async (IRepoGeneroAsync repo) => 
    await repo.Obtener()
        is List<Genero> ListaDeGeneros
            ? Results.Ok(ListaDeGeneros)
            : Results.NotFound());

app.MapGet("/genero/{id}", async (IRepoGeneroAsync repo, byte id) => 
    await repo.DetalleDe(id)
        is Genero genero 
            ? Results.Ok(genero)
            : Results.NotFound()

);

app.MapPost("/genero", async (IRepoGeneroAsync repo, Genero genero) => 
{  
    await repo.Alta(genero);

    return Results.Created($"/genero/{genero.idGenero}", genero);   
});

app.MapGet("/album/{id}", async(IRepoAlbumAsync repo, uint id) =>
    await repo.DetalleDe(id)
        is Album album
        ? Results.Ok(album)
        : Results.NotFound()
);
app.Run();