using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using TagLib;


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
                string? audioFileName = null;
                TimeSpan duracionAudio = TimeSpan.Zero;

                if (model.AudioUrl != null && model.AudioUrl.Length > 0)
                {
                    string audioFolder = Path.Combine(_env.WebRootPath, "Audios");
                    if (!Directory.Exists(audioFolder))
                        Directory.CreateDirectory(audioFolder);

                    audioFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.AudioUrl.FileName);
                    string audioPath = Path.Combine(audioFolder, audioFileName);

                    using (var stream = new FileStream(audioPath, FileMode.Create))
                    {
                        await model.AudioUrl.CopyToAsync(stream);
                    }

                    // *** Obtener duración real con TagLib ***
                    var audioTag = TagLib.File.Create(audioPath);
                    duracionAudio = audioTag.Properties.Duration;
                }

                // Crear objeto Cancion
                var cancion = new Cancion
                {
                    Titulo = model.Titulo,
                    duration = duracionAudio,
                    artista = albumSeleccionado.artista,
                    album = albumSeleccionado,
                    genero = generoSeleccionado,
                    ImageUrl = uniqueFileName,
                    AudioUrl = audioFileName
                };


                // Guardar en la BD
                var idAutoIncrementado = await repoCancion.Alta(cancion);

                TempData["SuccessMessage"] = "Canción creada correctamente";
                return RedirectToAction("Index","Home");
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
