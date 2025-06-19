namespace Spotify.Core.Persistencia;

public interface IEliminar <T>
{
    Task Eliminar (T elemento );
}
