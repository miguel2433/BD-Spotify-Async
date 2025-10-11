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

        private readonly IWebHostEnvironment _env;
        private readonly IRepoGeneroAsync repoGenero;
        private readonly IRepoCancionAsync repoCancion;

        public CancionController(
            ILogger<CancionController> logger,
            IRepoArtistaAsync repoArtista,
            IRepoAlbumAsync repoAlbum,
            IRepoGeneroAsync repoGenero,
            IRepoCancionAsync repoCancion,
            IWebHostEnvironment env)
        {
            _logger = logger;
            this.repoArtista = repoArtista;
            this.repoAlbum = repoAlbum;
            this.repoGenero = repoGenero;
            this._env = env;
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
            // Debug: mostrar errores de ModelState
            foreach (var kvp in ModelState)
            {
                foreach (var error in kvp.Value.Errors)
                {
                    Console.WriteLine($"[ModelState ERROR] {kvp.Key} -> {error.ErrorMessage}");
                }
            }

            // Validar campos obligatorios
            if (!ModelState.IsValid)
            {
                // Recargar combos antes de devolver la vista
                model.artistas = await repoArtista.Obtener();
                model.albums = await repoAlbum.Obtener();
                model.generos = await repoGenero.Obtener();
                return View(model);
            }

            try
            {
                // Traer entidades completas
                var albumSeleccionado = await repoAlbum.DetalleDe(model.AlbumId);
                var generoSeleccionado = await repoGenero.DetalleDe(model.GeneroId);

                // Procesar imagen si existe
                string? uniqueFileName = null;
                if (model.ImageUrl != null && model.ImageUrl.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageUrl.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageUrl.CopyToAsync(fileStream);
                    }
                }

                // Crear objeto Cancion
                var cancion = new Cancion
                {
                    Titulo = model.Titulo,
                    duration = new TimeSpan(0, 3, 0), // default 3 min
                    artista = albumSeleccionado.artista,
                    album = albumSeleccionado,
                    genero = generoSeleccionado,
                    ImageUrl = uniqueFileName
                };

                // Guardar en la BD
                var idAutoIncrementado = await repoCancion.Alta(cancion);

                TempData["SuccessMessage"] = "Canción creada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR CrearCancion] {ex.Message}");
                ModelState.AddModelError("", "Error al crear la canción: " + ex.Message);

                // Recargar combos antes de devolver la vista
                model.artistas = await repoArtista.Obtener();
                model.albums = await repoAlbum.Obtener();
                model.generos = await repoGenero.Obtener();

                return View(model);
            }
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
