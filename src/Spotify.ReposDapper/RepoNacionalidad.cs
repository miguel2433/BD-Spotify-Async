
namespace Spotify.ReposDapper;

public class RepoNacionalidad : RepoGenerico, IRepoNacionalidad
{
    public RepoNacionalidad(IDbConnection conexion) 
        : base(conexion) { }
    
    public async Task<uint> Alta (Nacionalidad nacionalidad )
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidNacionalidad", direction: ParameterDirection.Output);
        parametros.Add("@unPais", nacionalidad.Pais);

        await _conexion.ExecuteAsync("altaNacionalidad", parametros, commandType: CommandType.StoredProcedure);

        nacionalidad.idNacionalidad = parametros.Get<uint>("@unidNacionalidad");

        return nacionalidad.idNacionalidad;
    }

    public async Task<Nacionalidad?> DetalleDe(uint idNacionalidad)
    {
        var BuscarNacionalidadPorId = @"SELECT * FROM Nacionalidad WHERE idNacionalidad = @idNacionalidad";

        var Buscar = await _conexion.QueryFirstOrDefaultAsync<Nacionalidad>(BuscarNacionalidadPorId, new{idNacionalidad});

        return Buscar;
    }

    public IList<Nacionalidad> Obtener () => EjecutarSPConReturnDeTipoLista<Nacionalidad>("ObtenerNacionalidades").ToList();
}