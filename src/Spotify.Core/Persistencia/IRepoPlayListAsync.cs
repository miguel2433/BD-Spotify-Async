namespace Spotify.Core.Persistencia;
public interface IRepoPlaylistAsync : IAltaAsync<Playlist , uint>, IListadoAsync<Playlist>, IDetallePorIdAsync<Playlist,uint>,IMatcheoAsync
{
     Task<List<Playlist>> PlaylistsDelUsuario(uint  idUsuario);
}