using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoPlaylistAsync : RepoGenerico, IRepoPlaylistAsync
{
    public RepoPlaylistAsync(IDbConnection conexion) 
        : base(conexion) {}

    public async Task<uint> Alta(PlayList playlist)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidPlaylist", direction: ParameterDirection.Output);
        parametros.Add("@unNombre", playlist.Nombre);
        parametros.Add("@unidUsuario", playlist.usuario.idUsuario);


        await _conexion.ExecuteAsync("altaPlaylist", parametros, commandType: CommandType.StoredProcedure);

        playlist.idPlaylist = parametros.Get<uint>("@unidPlaylist");

        return playlist.idPlaylist;
    }

    public async Task<PlayList> DetalleDe(uint idPlaylist)
    {
        var BuscarPlayListPorId = @"SELECT * FROM Playlist WHERE idPlaylist = @idPlaylist";

        var Buscar = await _conexion.QueryFirstOrDefaultAsync<PlayList>(BuscarPlayListPorId, new {idPlaylist});

        return Buscar; 
    }

    public IList<PlayList> Obtener () => EjecutarSPConReturnDeTipoLista<PlayList>("ObtenerPlayLists").ToList();
    
    public async Task<IList<Cancion>?> DetallePlaylist(uint idPlaylist)
    {
        var consultaExistenciaPlaylist = "SELECT COUNT(*) FROM Playlist WHERE idPlaylist = @idPlaylist";
        var noExiste = await  _conexion.ExecuteScalarAsync<int>(consultaExistenciaPlaylist, new { idPlaylist }) == 0;

        if (noExiste)
        {
            return null; 
        }

        var query = @"
            SELECT c.* 
            FROM Cancion c
            JOIN Cancion_Playlist cp ON c.idCancion = cp.idCancion
            WHERE cp.idPlaylist = @idPlaylist";

        var canciones = await _conexion.QueryAsync<Cancion>(query, new { idPlaylist });

        
        return canciones.ToList(); 
    }
} 
