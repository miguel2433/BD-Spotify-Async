using Microsoft.AspNetCore.Mvc;
using Spotify.Core;
using Spotify.Core.Persistencia;

namespace SpotifyMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRepoCancionAsync _repoCancion;
        private readonly IRepoAlbumAsync _repoAlbum;
        private readonly IRepoArtistaAsync _repoArtista;
        private readonly IRepoPlaylistAsync _repoPlaylist;

        public SearchController(
            IRepoCancionAsync repoCancion,
            IRepoAlbumAsync repoAlbum,
            IRepoArtistaAsync repoArtista,
            IRepoPlaylistAsync repoPlaylist)
        {
            _repoCancion = repoCancion;
            _repoAlbum = repoAlbum;
            _repoArtista = repoArtista;
            _repoPlaylist = repoPlaylist;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            var vm = new SearchViewModel { Query = q ?? string.Empty };

            if (string.IsNullOrWhiteSpace(q))
            {
                return View(vm);
            }

            string term = q.Trim().ToLowerInvariant();

            // Canciones (con joins completos)
            var canciones = await _repoCancion.ObtenerTodo();
            vm.Canciones = canciones
                .Where(c =>
                    (!string.IsNullOrEmpty(c.Titulo) && c.Titulo.ToLower().Contains(term)) ||
                    (c.artista != null && !string.IsNullOrEmpty(c.artista.NombreArtistico) && c.artista.NombreArtistico.ToLower().Contains(term)) ||
                    (c.album != null && !string.IsNullOrEmpty(c.album.Titulo) && c.album.Titulo.ToLower().Contains(term)))
                .Take(20)
                .ToList();

            // Álbumes (con artista)
            var albums = await _repoAlbum.ObtenerTodo();
            vm.Albums = albums
                .Where(a =>
                    (!string.IsNullOrEmpty(a.Titulo) && a.Titulo.ToLower().Contains(term)) ||
                    (a.artista != null && !string.IsNullOrEmpty(a.artista.NombreArtistico) && a.artista.NombreArtistico.ToLower().Contains(term)))
                .Take(20)
                .ToList();

            // Artistas
            var artistas = await _repoArtista.Obtener();
            vm.Artistas = artistas
                .Where(ar =>
                    (!string.IsNullOrEmpty(ar.NombreArtistico) && ar.NombreArtistico.ToLower().Contains(term)) ||
                    (!string.IsNullOrEmpty(ar.Nombre) && ar.Nombre.ToLower().Contains(term)) ||
                    (!string.IsNullOrEmpty(ar.Apellido) && ar.Apellido.ToLower().Contains(term)))
                .Take(20)
                .ToList();

            // Playlists (todas)
            var playlists = await _repoPlaylist.Obtener();
            vm.Playlists = playlists
                .Where(p => !string.IsNullOrEmpty(p.Nombre) && p.Nombre.ToLower().Contains(term))
                .Take(20)
                .ToList();

            return View(vm);
        }
    }
}
