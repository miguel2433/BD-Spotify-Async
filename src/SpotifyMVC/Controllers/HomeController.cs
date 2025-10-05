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
    private readonly IRepoCancionAsync repoCancion;
    public HomeController(ILogger<HomeController> logger,IRepoCancionAsync repoCancion, IRepoArtistaAsync repoArtista, IRepoAlbumAsync repoAlbum)
    {
        this.repoAlbum = repoAlbum;
        this.repoArtista = repoArtista;
        this.repoCancion = repoCancion;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
            artistas = await repoArtista.Obtener(),
            albunes = await repoAlbum.Obtener(),
            canciones = await repoCancion.Obtener()
        };
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
