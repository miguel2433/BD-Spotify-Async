namespace Spotify.Core.Persistencia;

public interface IRepoCancionAsync : IAltaAsync<Cancion, uint>, IListadoTotalAsync<Cancion>,IListadoAsync<Cancion>, IDetallePorIdAsync<Cancion,uint>, IMatcheoAsync
{ }