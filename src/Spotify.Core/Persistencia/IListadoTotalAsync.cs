namespace Spotify.Core.Persistencia;
public interface IListadoTotalAsync<T>
{
    Task<List<T>> ObtenerTodo();
}