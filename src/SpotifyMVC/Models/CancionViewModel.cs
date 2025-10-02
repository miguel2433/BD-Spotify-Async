using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class CancionViewModel
    {
         public List<Cancion> canciones { get; set; } = new();
    }
}