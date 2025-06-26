namespace Spotify.Core.Persistencia;

public interface IAltaAsync<T, N>
{
    Task<N> Alta(T elemento);
}