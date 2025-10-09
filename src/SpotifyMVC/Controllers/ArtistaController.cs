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
    private readonly IRepoCancionAsync repoCancion;
    private readonly IWebHostEnvironment _env;
    public ArtistaController(ILogger<ArtistaController> logger, IWebHostEnvironment env,IRepoCancionAsync repoCancion, IRepoArtistaAsync repoArtista, IRepoAlbumAsync repoAlbum)
    {
        this.repoAlbum = repoAlbum;
        this.repoArtista = repoArtista;
        this.repoCancion = repoCancion;
        _logger = logger;
        _env = env;
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
            return NotFound();
        }
        var albums = await repoAlbum.ObtenerTodo();
        var albums_del_artista = albums.Where(album => album.artista.idArtista == artista.idArtista).ToList();
        
        var canciones = await repoCancion.ObtenerTodo();
        var canciones_del_artista = canciones.Where(cancion => cancion.artista.idArtista == artista.idArtista).ToList();

        var vm = new DetalleArtistaViewModel
        {
            artista = artista,
            albums = albums_del_artista,
            canciones = canciones_del_artista
        };

        return View(vm);
    }

    public IActionResult CrearArtista() => View();


    [HttpPost]
    public async Task<IActionResult> CrearArtista(CrearArtistaViewModel model)
    {
        var artista = model.artista;

        var image = model.ImageUrl;

        string uniqueFileName = null;

        if (image != null && image.Length > 0)
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                artista.ImageUrl = uniqueFileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar la imagen: " + ex.Message);
                return View(model);
            }
        }
        try
            {
                var idAutoIncrementado = await repoArtista.Alta(artista);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el Artista: " + ex.Message);
                return View(model);
            }


        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
