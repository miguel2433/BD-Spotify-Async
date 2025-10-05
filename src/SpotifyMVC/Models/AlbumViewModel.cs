using Microsoft.AspNetCore.Mvc.Rendering;
using Spotify.Core;

namespace SpotifyMVC.Models;

public class AlbumViewModel
{
    public List<Artista> artistas { get; set; } = new();
    public string Titulo { get; set; }
    public DateTime FechaLanzamiento { get; set; } = DateTime.Now;
    public IFormFile ImageUrl { get; set; }
    public uint ArtistaId { get; set; }

}
