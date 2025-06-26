namespace Spotify.Core.Persistencia;

public interface IRepoGeneroAsync : IAltaAsync<Genero, byte>, IListado<Genero>, IEliminarAsync<uint>, IDetallePorIdAsync<Genero,byte>
{ }