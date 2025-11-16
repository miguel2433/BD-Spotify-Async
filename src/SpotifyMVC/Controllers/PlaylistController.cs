using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core;
using Spotify.Core.Persistencia;
using Spotify.ReposDapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;



namespace SpotifyMVC.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly ILogger<PlaylistController> _logger;
        private readonly IRepoPlaylistAsync repoPlaylist;
        private readonly IRepoUsuarioAsync repoUsuario;
        private readonly IRepoCancionAsync repoCancion;
        private readonly IWebHostEnvironment _env;

        public PlaylistController(
            ILogger<PlaylistController> logger,
            IRepoPlaylistAsync repoPlaylist,
            IWebHostEnvironment env,
            IRepoUsuarioAsync repoUsuario,
            IRepoCancionAsync repoCancion)
        {
            _logger = logger;
            this.repoPlaylist = repoPlaylist;
            _env = env;
            this.repoUsuario = repoUsuario;
            this.repoCancion = repoCancion;
        }


        [HttpGet]

        public IActionResult CrearPlaylist() => View();
 
    [HttpPost]
    public async Task<IActionResult> CrearPlaylist(CrearPlaylistViewModel model)
    {
        var idUsuarioString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (idUsuarioString == null)
        {
            ModelState.AddModelError("", "No se pudo obtener el id del usuario logueado.");
            return View(model);
        }

        uint idUsuario = uint.Parse(idUsuarioString);    

        Usuario usuario = await repoUsuario.DetalleDe(idUsuario);

        var playlist = model.playlist;

        playlist.usuario = usuario;

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
                playlist.ImageUrl = uniqueFileName;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar la imagen: " + ex.Message);
                return View(model);
            }
        }
        try
            {
                var idAutoIncrementado = await repoPlaylist.Alta(playlist);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear el Artista: " + ex.Message);
                return View(model);
            }


        return RedirectToAction("Index","Home");
    }
        public async Task<IActionResult> DetallePlaylist(uint id)
        {
            var playList = await repoPlaylist.DetalleDe(id);
            
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }
        [HttpGet]
        public async Task<IActionResult> AgregarCancion(uint idPlaylist)
        {
            var playlist = await repoPlaylist.DetalleDe(idPlaylist);
            if (playlist == null)
            {
                return NotFound();
            }

            // Usar ObtenerTodo() en lugar de Obtener() para cargar las relaciones
            var canciones = await repoCancion.ObtenerTodo(); 
            if (canciones == null || !canciones.Any())
            {
                return NotFound("No se encontraron canciones");
            }

            var vm = new AgregarCancionAPlaylistViewModel
            {
                playlist = playlist,
                TodasLasCanciones = canciones
            };

            return PartialView("_ModalAgregarCancion", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarCancion(AgregarCancionAPlaylistViewModel model)
        {
            await repoPlaylist.InsertarEnPlaylistCancion(model.playlist.idPlaylist, model.IdCancionSeleccionada);

            return RedirectToAction("DetallePlaylist", new { id = model.playlist.idPlaylist });
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string term)
        {
            var idUsuarioString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(idUsuarioString))
            {
                return Unauthorized();
            }

            uint idUsuario = uint.Parse(idUsuarioString);

            var playlists = await repoPlaylist.PlaylistsDelUsuario(idUsuario);

            if (!string.IsNullOrWhiteSpace(term))
            {
                var t = term.Trim().ToLowerInvariant();
                playlists = playlists
                    .Where(p => !string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Contains(t))
                    .ToList();
            }

            return PartialView("_PlaylistsList", playlists);
        }
    }

}
