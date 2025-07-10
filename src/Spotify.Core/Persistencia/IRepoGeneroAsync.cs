namespace Spotify.Core.Persistencia;

public interface IRepoGeneroAsync : IAltaAsync<Genero, byte>, IListadoAsync<Genero>, IEliminarAsync<uint>, IDetallePorIdAsync<Genero,byte>
{ }