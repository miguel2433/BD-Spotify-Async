namespace Spotify.Core.Persistencia;
public interface IListadoAsync<T>
{
    Task<IEnumerable<T>> Obtener();
}