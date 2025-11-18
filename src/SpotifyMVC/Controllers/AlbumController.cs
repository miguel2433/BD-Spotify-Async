using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using Microsoft.AspNetCore.Authorization;



namespace SpotifyMVC.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IRepoArtistaAsync repoArtista;
        private readonly IRepoCancionAsync repoCancion;
        private readonly IRepoAlbumAsync repoAlbum;
        private readonly IWebHostEnvironment _env;

        public AlbumController(
            ILogger<AlbumController> logger,
            IRepoArtistaAsync repoArtista,
            IRepoAlbumAsync repoAlbum,
            IRepoCancionAsync repoCancion,
            IWebHostEnvironment env)
        {
            _logger = logger;
            this.repoArtista = repoArtista;
            this.repoAlbum = repoAlbum;
            this.repoCancion = repoCancion;
            this._env = env;
        }

        // GET: mostrar formulario
        public async Task<IActionResult> Index()
        {

            var albums = await repoAlbum.Obtener();

            return View(albums);
        }

        // POST: dar de alta álbum
        [HttpPost]
        public async Task<IActionResult> CrearAlbum(AlbumViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Traer el artista completo por su ID
                var artistaSeleccionado = await repoArtista.DetalleDe(model.ArtistaId);

                var image = model.ImageUrl;

                string uniqueFileName = null;

                //Crear Imagen
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
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error al guardar la imagen: " + ex.Message);
                        return View(model);
                    }
                }
                var album = new Album
                {
                    Titulo = model.Titulo,
                    fechaLanzamiento = model.FechaLanzamiento,
                    artista = artistaSeleccionado,
                    ImageUrl = uniqueFileName
                };
                try
                {
                    var idAutoIncrementado = await repoAlbum.Alta(album);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear el Artista: " + ex.Message);
                    return View(model);
                }
                return RedirectToAction("Index","Home");
            }

            // Si hay error, recargar lista de artistas
            model.artistas = await repoArtista.Obtener();
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> CrearAlbum()
        {

            var vm = new AlbumViewModel
            {
                artistas = await repoArtista.Obtener()
            };
            return View(vm);
        }

        public async Task<IActionResult> DetalleAlbum(uint id)
        {
            var album = await repoAlbum.DetalleDe(id);

            var Canciones = await repoCancion.ObtenerTodo();
            var canciones_del_album = Canciones.Where(cancion => cancion.album.idAlbum == album.idAlbum).ToList();

            var ArtistaDelAlbum = await repoArtista.DetalleDe(album.artista.idArtista);

            var vm = new DetalleAlbumViewModel
            {
                canciones = canciones_del_album,
                artista = ArtistaDelAlbum,
                album = album
            };

            return View(vm);
        }

    }
}
