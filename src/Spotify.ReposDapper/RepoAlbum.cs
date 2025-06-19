namespace Spotify.ReposDapper;

public class RepoAlbum : RepoGenerico, IRepoAlbum
{
    public RepoAlbum(IDbConnection conexion) 
        : base(conexion) { }

    public async Task<uint> Alta(Album album)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidAlbum", direction: ParameterDirection.Output);
        parametros.Add("@unTitulo", album.Titulo);
        parametros.Add("@unidArtista", album.artista.idArtista);

        await _conexion.ExecuteAsync("altaAlbum", parametros, commandType: CommandType.StoredProcedure);
        album.idAlbum = parametros.Get<uint>("@unidAlbum");

        return album.idAlbum;
    }

    public async Task<Album?> DetalleDe(uint idAlbum)
    {
        string consultarAlbum = @"SELECT * FROM Album WHERE idAlbum = @idAlbum";

        var album = await _conexion.QuerySingleOrDefaultAsync<Album>(consultarAlbum, new {idAlbum});

        return album;
    }

    public async Task Eliminar(uint idAlbum)
    {
        string eliminarCanciones = @"DELETE FROM Cancion WHERE idAlbum = @idAlbum";
        await _conexion.ExecuteAsync(eliminarCanciones, new {idAlbum});

        string eliminarAlbum = @"DELETE FROM Album WHERE idAlbum = @idAlbum";
        await _conexion.ExecuteAsync(eliminarAlbum, new {idAlbum});
    }

    public IList<Album> Obtener() => EjecutarSPConReturnDeTipoLista<Album>("ObtenerAlbum").ToList();


}
