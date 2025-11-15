
namespace Spotify.ReposDapper;

public class RepoPlaylist : RepoGenerico, IRepoPlaylist
{
    public RepoPlaylist(IDbConnection conexion) 
        : base(conexion) {}

    public  uint Alta(Playlist playlist)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidPlaylist", direction: ParameterDirection.Output);
        parametros.Add("@unNombre", playlist.Nombre);
        parametros.Add("@unidUsuario", playlist.usuario.idUsuario);
        parametros.Add("@unImageUrl", playlist.ImageUrl);


         _conexion.Execute("altaPlaylist", parametros, commandType: CommandType.StoredProcedure);

        playlist.idPlaylist = parametros.Get<uint>("@unidPlaylist");

        return playlist.idPlaylist;
    }

    public  Playlist DetalleDe(uint idPlaylist)
    {
        var BuscarPlaylistPorId = @"SELECT * FROM Playlist WHERE idPlaylist = @idPlaylist";

        var Buscar =  _conexion.QueryFirstOrDefault<Playlist>(BuscarPlaylistPorId, new {idPlaylist});

        return Buscar; 
    }

    public List<Playlist> Obtener () => EjecutarSPConReturnDeTipoLista<Playlist>("ObtenerPlaylists").ToList();
    
    public  IList<Cancion>? DetallePlaylist(uint idPlaylist)
    {
        var consultaExistenciaPlaylist = "SELECT COUNT(*) FROM Playlist WHERE idPlaylist = @idPlaylist";
        var noExiste =   _conexion.ExecuteScalar<int>(consultaExistenciaPlaylist, new { idPlaylist }) == 0;

        if (noExiste)
        {
            return null; 
        }

        var query = @"
            SELECT c.* 
            FROM Cancion c
            JOIN Cancion_Playlist cp ON c.idCancion = cp.idCancion
            WHERE cp.idPlaylist = @idPlaylist";

        var canciones =  _conexion.Query<Cancion>(query, new { idPlaylist });

        
        return canciones.ToList(); 
    }
} 
