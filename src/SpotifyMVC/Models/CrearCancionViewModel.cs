using System.ComponentModel.DataAnnotations;
using Spotify.Core;

public class CrearCancionViewModel
{
    [Required(ErrorMessage = "El título es obligatorio")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un artista")]
    public uint ArtistaId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un álbum")]
    public uint AlbumId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un género")]
    public byte GeneroId { get; set; }

    public IFormFile? ImageUrl { get; set; }
    public IFormFile? AudioUrl { get; set; }

    public List<Artista> artistas { get; set; } = new();
    public List<Album> albums { get; set; } = new();
    public List<Genero> generos { get; set; } = new();
}
