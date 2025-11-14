namespace Spotify.ReposDapper;
public class RepoCancionAsync : RepoGenerico, IRepoCancionAsync
{
    public RepoCancionAsync(IDbConnection conexion)
        : base(conexion) { }

    public async Task<uint> Alta(Cancion cancion)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidCancion", direction: ParameterDirection.Output);
        parametros.Add("@unTitulo", cancion.Titulo);
        parametros.Add("@unDuration", cancion.duration);
        parametros.Add("@unidAlbum", cancion.album.idAlbum);
        parametros.Add("@unidArtista", cancion.artista.idArtista);
        parametros.Add("@unidGenero", cancion.genero.idGenero);
        parametros.Add("@unImageUrl", cancion.ImageUrl);
        parametros.Add("@unAudioUrl", cancion.AudioUrl);

        await _conexion.ExecuteAsync("altaCancion", parametros, commandType: CommandType.StoredProcedure);

        cancion.idCancion = parametros.Get<uint>("@unidCancion");

        return cancion.idCancion;
    }

public async Task<Cancion?> DetalleDe(uint idCancion)
{
    string sql = @"
        SELECT *
        FROM Cancion c
        JOIN Artista ar ON c.idArtista = ar.idArtista
        JOIN Album a ON c.idAlbum = a.idAlbum
        JOIN Genero g ON c.idGenero = g.idGenero
        WHERE c.idCancion = @idCancion;
    ";

    var resultado = await _conexion.QueryAsync<Cancion, Artista, Album, Genero, Cancion>(
        sql,
        (cancion, artista, album, genero) =>
        {
            cancion.artista = artista;
            cancion.album = album;
            cancion.genero = genero;
            return cancion;
        },
        new { idCancion },
        splitOn: "idArtista,idAlbum,idGenero"
    );

    return resultado.FirstOrDefault();
}


    public async Task<List<string>?> Matcheo(string Cadena)
    {
        var parametro = new DynamicParameters();
        parametro.Add("@InputCancion", Cadena);

        var Lista = await _conexion.QueryAsync<string>("MatcheoCancion", parametro, commandType: CommandType.StoredProcedure);

        return Lista.ToList();
    }


    public async Task<List<Cancion>> Obtener() {
        var task = await EjecutarSPConReturnDeTipoListaAsync<Cancion>("ObtenerCanciones");
        return task.ToList();
        }

    public async Task<List<Cancion>> ObtenerTodo()
    {
        string sql = @"
            SELECT *
            FROM Cancion c
            JOIN Artista ar ON c.idArtista = ar.idArtista
            JOIN Album a ON c.idAlbum = a.idAlbum
            JOIN Genero g ON c.idGenero = g.idGenero
            ORDER BY c.Titulo ASC;
        ";

        var resultado = await _conexion.QueryAsync<Cancion, Artista, Album, Genero, Cancion>(
            sql,
            (cancion, artista, album, genero) =>
            {
                cancion.artista = artista;
                cancion.album = album;
                cancion.genero = genero;
                return cancion;
            },
            splitOn: "idArtista,idAlbum,idGenero"
        );

        return resultado.ToList();
    }

}
