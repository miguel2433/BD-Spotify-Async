namespace Spotify.Core.Persistencia;

public interface IAlta<T, N>
{
    Task<N> Alta(T elemento);
}