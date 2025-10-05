using System.Diagnostics.CodeAnalysis;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class CrearCancionViewModel
    {
        public string Titulo { get; set; }
        public TimeSpan Duracion { get; set; } = new TimeSpan();
        public string ImageUrl { get; set; }
        public List<Artista> artistas { get; set; } = new();
        public List<Album> albums { get; set; } = new();
        public List<Genero> generos { get; set; } = new();
        public uint ArtistaId { get; set; }
        public uint AlbumId { get; set; }
        public byte GeneroId { get; set; }
    }
}
