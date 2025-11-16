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
        parametros.Add("@unImageUrl", playlist.ImageUrl);
        parametros.Add("@unNombre", playlist.Nombre);
        parametros.Add("@unidUsuario", playlist.usuario.idUsuario);


        await _conexion.ExecuteAsync("altaPlaylist", parametros, commandType: CommandType.StoredProcedure);

        playlist.idPlaylist = parametros.Get<uint>("@unidPlaylist");

        return playlist.idPlaylist;
    }

    public async Task<Playlist> DetalleDe(uint idPlaylist)
    {
        // Obtener la playlist con los datos del usuario
        var sql = @"
            SELECT p.*, u.* 
            FROM Playlist p
            INNER JOIN Usuario u ON p.idUsuario = u.idUsuario
            WHERE p.idPlaylist = @idPlaylist";
        
        var playlist = (await _conexion.QueryAsync<Playlist, Usuario, Playlist>(
            sql,
            (playlist, usuario) => 
            {
                playlist.usuario = usuario;
                return playlist;
            },
            new { idPlaylist },
            splitOn: "idUsuario"
        )).FirstOrDefault();

        if (playlist != null)
        {
            // Cargar las canciones de la playlist
            playlist.Canciones = await CancionesDeLaPlaylist(idPlaylist);
        }

        return playlist;
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

    public async Task<List<Cancion>> CancionesDeLaPlaylist(uint idPlaylist)
    {
        var parametro = new DynamicParameters();
        parametro.Add("@unIdPlaylist", idPlaylist);

        var sql = @"
            SELECT 
                c.idCancion, c.Titulo, c.duration, c.AudioUrl,
                a.idArtista, a.NombreArtistico
            FROM Cancion_Playlist cp
            INNER JOIN Cancion c ON c.idCancion = cp.idCancion
            INNER JOIN Artista a ON a.idArtista = c.idArtista
            WHERE cp.idPlaylist = @unIdPlaylist;
        ";

        var lista = await _conexion.QueryAsync<Cancion, Artista, Cancion>(
            sql,
            (cancion, artista) =>
            {
                cancion.artista = artista;
                return cancion;
            },
            param: parametro,
            splitOn: "idArtista"
        );

        return lista.ToList();
    }

    public async Task InsertarEnPlaylistCancion(uint idPlaylist, uint idCancion)
    {
        var sql = @"INSERT INTO Cancion_Playlist(idPlaylist, idCancion)
                    VALUES (@idPlaylist, @idCancion)";

        var parametros = new
        {
            idPlaylist,
            idCancion
        };

        await _conexion.ExecuteAsync(sql, parametros);
    }

} 
