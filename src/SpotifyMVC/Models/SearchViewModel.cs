using Spotify.Core;

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;

    public List<Cancion> Canciones { get; set; } = new();
    public List<Album> Albums { get; set; } = new();
    public List<Artista> Artistas { get; set; } = new();
    public List<Playlist> Playlists { get; set; } = new();
}
