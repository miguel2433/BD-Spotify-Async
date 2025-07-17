using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoAlbumAsync : RepoGenerico, IRepoAlbumAsync
{
    public RepoAlbumAsync(IDbConnection conexion) 
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
    string sql = @"
        SELECT 
            a.idAlbum, a.Titulo, a.idArtista,
            ar.idArtista, ar.Nombre, ar.Apellido, ar.NombreArtistico
        FROM Album a
        JOIN Artista ar ON a.idArtista = ar.idArtista
        WHERE a.idAlbum = @idAlbum";

    var resultado = await _conexion.QueryAsync<Album, Artista, Album>(
        sql,
        (album, artista) =>
        {
            album.artista = artista;
            return album;
        },
        new { idAlbum },
        splitOn: "idArtista" // importante para que Dapper sepa dónde empieza el segundo objeto
    );

    return resultado.FirstOrDefault();
}

    public async Task Eliminar(uint idAlbum)
    {
        string eliminarCanciones = @"DELETE FROM Cancion WHERE idAlbum = @idAlbum";
        await _conexion.ExecuteAsync(eliminarCanciones, new {idAlbum});

        string eliminarAlbum = @"DELETE FROM Album WHERE idAlbum = @idAlbum";
        await _conexion.ExecuteAsync(eliminarAlbum, new {idAlbum});
    }

    public async Task<List<Album>> Obtener() { 
        var task = await EjecutarSPConReturnDeTipoListaAsync<Album>("ObtenerAlbum");
        return task.ToList();
        }


}
