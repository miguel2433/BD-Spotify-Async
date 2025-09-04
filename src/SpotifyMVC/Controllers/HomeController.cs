using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;

namespace BD_Sporify._MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepoArtistaAsync repoArtista;
    public HomeController(ILogger<HomeController> logger,IRepoArtistaAsync repoArtista)
    {
        this.repoArtista = repoArtista;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {   
        var artistas = await repoArtista.Obtener();
        return View(artistas);
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