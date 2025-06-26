namespace Spotify.Core.Persistencia;

public interface IEliminarAsync <T>
{
    Task Eliminar (T elemento );
}
