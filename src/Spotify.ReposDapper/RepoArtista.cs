using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoArtista : RepoGenerico, IRepoArtista
{
    public RepoArtista(IDbConnection conexion) 
        : base(conexion) { }

    public async Task<uint> Alta(Artista artista)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidArtista", direction: ParameterDirection.Output);
        parametros.Add("@unNombreArtistico", artista.NombreArtistico);
        parametros.Add("@unNombre", artista.Nombre);
        parametros.Add("@unApellido", artista.Apellido);

        await _conexion.ExecuteAsync("altaArtista", parametros, commandType: CommandType.StoredProcedure);

        artista.idArtista = parametros.Get<uint>("@unidArtista");

        return artista.idArtista;
    }

    public async Task Eliminar(uint idArtista)
    {
        string eliminarAlbum = @"DELETE FROM Album WHERE idArtista = @idArtista";
        await _conexion.ExecuteAsync(eliminarAlbum, new { idArtista });
        
        string eliminarArtista = @"DELETE FROM Artista WHERE idArtista = @idArtista";
        await _conexion.ExecuteAsync(eliminarArtista, new { idArtista });
    }

    public IList<Artista> Obtener() => EjecutarSPConReturnDeTipoLista<Artista>("ObtenerArtistas").ToList();

    public async Task<Artista?> DetalleDe(uint idArtista)
    {
        string consultarArtista = @"SELECT * FROM Artista WHERE idArtista = @idArtista";

        var artista = await _conexion.QuerySingleOrDefaultAsync<Artista>(consultarArtista, new {idArtista});

        return artista;
    }
}