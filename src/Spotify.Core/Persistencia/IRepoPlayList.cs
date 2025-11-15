namespace Spotify.Core.Persistencia;
public interface IRepoPlaylist : IAlta<Playlist , uint>, IListado<Playlist>, IDetallePorId<Playlist,uint>
{ }