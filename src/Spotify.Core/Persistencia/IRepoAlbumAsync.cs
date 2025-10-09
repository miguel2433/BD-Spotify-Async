namespace Spotify.Core.Persistencia;

public interface IRepoAlbumAsync : IAltaAsync<Album, uint>,IListadoTotalAsync<Album>, IListadoAsync<Album>, IEliminarAsync<uint>, IDetallePorIdAsync<Album, uint>
{}