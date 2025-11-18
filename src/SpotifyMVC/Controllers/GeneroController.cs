using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using Spotify.Core;

namespace BD_Sporify._MVC.Controllers;

public class GeneroController : Controller
{
    private readonly ILogger<GeneroController> _logger;
    private readonly IRepoGeneroAsync repoGenero;
    public GeneroController(ILogger<GeneroController> logger, IRepoGeneroAsync repoGenero)
    {
        this.repoGenero = repoGenero;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var generos = await repoGenero.Obtener();

        return View(generos);
    }

    public IActionResult CrearGenero() => View();


    [HttpPost]
    public async Task<IActionResult> CrearGenero(Genero generoCrear)
    {
        await repoGenero.Alta(generoCrear);

        return RedirectToAction("Index","Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
