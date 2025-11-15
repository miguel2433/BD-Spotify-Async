using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoPlaylistAsync : RepoGenerico, IRepoPlaylistAsync
{
    public RepoPlaylistAsync(IDbConnection conexion) 
        : base(conexion) {}

    public async Task<uint> Alta(Playlist playlist)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidPlaylist", direction: ParameterDirection.Output);
        parametros.Add("@unNombre", playlist.Nombre);
        parametros.Add("@unidUsuario", playlist.usuario.idUsuario);


        await _conexion.ExecuteAsync("altaPlaylist", parametros, commandType: CommandType.StoredProcedure);

        playlist.idPlaylist = parametros.Get<uint>("@unidPlaylist");

        return playlist.idPlaylist;
    }

    public async Task<Playlist> DetalleDe(uint idPlaylist)
    {
        var BuscarPlaylistPorId = @"SELECT * FROM Playlist WHERE idPlaylist = @idPlaylist";

        var Buscar = await _conexion.QueryFirstOrDefaultAsync<Playlist>(BuscarPlaylistPorId, new {idPlaylist});

        return Buscar; 
    }

    public async Task<List<Playlist>> Obtener () { 
        var task = await EjecutarSPConReturnDeTipoListaAsync<Playlist>("ObtenerPlaylists");
        return task.ToList();
    }

    
    public async Task<List<Cancion>?> DetallePlaylist(uint idPlaylist)
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

    public async Task<List<string>?> Matcheo(string Cadena)
    {
        var parametro = new DynamicParameters();
        parametro.Add("@InputPlaylist", Cadena);

        var Lista = await _conexion.QueryAsync<string>("MatcheoPlaylist", parametro, commandType: CommandType.StoredProcedure);

        return Lista.ToList();
    }

    public async Task<List<Playlist>> PlaylistsDelUsuario(uint idusuario)
    {
        var parametro = new DynamicParameters();
        parametro.Add("@unidUsuario", idusuario);

        var lista = await _conexion.QueryAsync<Playlist>(
            "PlaylistsDeUsuario",
            parametro,
            commandType: CommandType.StoredProcedure
        );

        return lista.ToList();
    }

} 
