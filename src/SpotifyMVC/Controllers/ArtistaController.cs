using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using Spotify.Core;

namespace BD_Sporify._MVC.Controllers;

public class ArtistaController : Controller
{
    private readonly ILogger<ArtistaController> _logger;
    private readonly IRepoArtistaAsync repoArtista;
    private readonly IRepoAlbumAsync repoAlbum;
    public ArtistaController(ILogger<ArtistaController> logger, IRepoArtistaAsync repoArtista, IRepoAlbumAsync repoAlbum)
    {
        this.repoAlbum = repoAlbum;
        this.repoArtista = repoArtista;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var artistas = await repoArtista.Obtener();

        return View(artistas);
    }
    public async Task<IActionResult> DetalleArtista(uint id)
    {
        var artista = await repoArtista.DetalleDe(id);

        if (artista == null)
        {
            return NotFound(); // o podrías devolver una vista personalizada tipo "ArtistaNoEncontrado"
        }

        return View(artista);
    }

    public IActionResult CrearArtista() => View();


    [HttpPost]
    public async Task<IActionResult> CrearArtista(Artista artista)
    {
        var altaArtista = await repoArtista.Alta(artista);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
