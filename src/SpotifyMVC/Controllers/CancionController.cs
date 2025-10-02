using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;

namespace SpotifyMVC.Controllers
{
    public class CancionController : Controller
    {
        private readonly ILogger<CancionController> _logger;
        private readonly IRepoArtistaAsync repoArtista;
        private readonly IRepoAlbumAsync repoAlbum;

        private readonly IRepoGeneroAsync repoGenero;
        private readonly IRepoCancionAsync repoCancion;

        public CancionController(
            ILogger<CancionController> logger,
            IRepoArtistaAsync repoArtista,
            IRepoAlbumAsync repoAlbum,
            IRepoGeneroAsync repoGenero,
            IRepoCancionAsync repoCancion)
        {
            _logger = logger;
            this.repoArtista = repoArtista;
            this.repoAlbum = repoAlbum;
            this.repoGenero = repoGenero;
            this.repoCancion = repoCancion;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new CancionViewModel
            {
                canciones = await repoCancion.Obtener()
            };
            return View(vm);
        }

        // POST: dar de alta Cancion
        [HttpPost]
        public async Task<IActionResult> CrearCancion(CrearCancionViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Traer el artista completo por su ID
                var artistaSeleccionado = await repoArtista.DetalleDe(model.ArtistaId);
                var albumSeleccionado = await repoAlbum.DetalleDe(model.AlbumId);
                var generoSeleccionado = await repoGenero.DetalleDe(model.GeneroId);

                // Crear el Album usando el objeto artista
                var Cancion = new Cancion
                {
                    Titulo = model.Titulo,
                    duration = new TimeSpan(0, 12, 5),
                    artista = artistaSeleccionado,
                    genero = generoSeleccionado,
                    album = albumSeleccionado
                };

                await repoCancion.Alta(Cancion);
                return RedirectToAction("Index");
            }
            return View();

        }
        [HttpGet]

        public async Task<IActionResult> CrearCancion()
        {
            var vm = new CrearCancionViewModel
            {
                albums = await repoAlbum.Obtener(),
                artistas = await repoArtista.Obtener(),
                generos = await repoGenero.Obtener()
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> DetalleCancion(uint id)
        {
            var Detalle = await repoCancion.DetalleDe(id);
            return View(Detalle);
        }
    }
}
