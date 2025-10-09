using Microsoft.AspNetCore.Mvc.Rendering;
using Spotify.Core;

namespace SpotifyMVC.Models;

public class DetalleArtistaViewModel
{
    public Artista artista { get; set; }
    public List<Album> albums { get; set; }
    public List<Cancion> canciones { get; set; }

}
