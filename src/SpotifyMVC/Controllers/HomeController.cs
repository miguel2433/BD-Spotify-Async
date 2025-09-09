using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;

namespace BD_Sporify._MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepoArtistaAsync repoArtista;
    private readonly IRepoAlbumAsync repoAlbum;
    public HomeController(ILogger<HomeController> logger, IRepoArtistaAsync repoArtista, IRepoAlbumAsync repoAlbum)
    {
        this.repoAlbum = repoAlbum;
        this.repoArtista = repoArtista;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var artistas = await repoArtista.Obtener();

        var vm = new ArtistaViewModel
        {
            artistas = artistas,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ArtistaViewModel model)
    {
        var altaArtista = await repoArtista.Alta(model.artista);

        model.artistas = await repoArtista.Obtener();

        return View(model);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}