namespace Spotify.Core;

public class Playlist
{
    public uint idPlaylist { get; set; }
    public required string Nombre { get; set; }
    public required Usuario usuario { get; set; }
    public required List<Cancion>? Canciones { get; set; }
    public required string ImageUrl { get; set; }
}
