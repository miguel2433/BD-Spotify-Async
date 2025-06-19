namespace Spotify.Core.Persistencia;

public interface IMatcheo
{
    public Task<List<string>?> Matcheo(string Cadena);
}
