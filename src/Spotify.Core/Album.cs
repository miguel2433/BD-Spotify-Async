namespace Spotify.Core
{
    public class Album
    {
        public uint idAlbum { get; set; }
        public required string Titulo { get; set; }
        public DateTime fechaLanzamiento { get; set; }
        public required Artista artista { get; set; }
        public required string ImageUrl { get; set; }
    }
}
