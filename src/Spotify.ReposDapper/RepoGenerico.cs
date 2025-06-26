namespace Spotify.ReposDapper;
public abstract class RepoGenerico
{
    protected readonly IDbConnection _conexion;
    protected RepoGenerico(IDbConnection conexion) => _conexion = conexion;
    protected async Task EjecutarSPSinReturn(string nombreSP, DynamicParameters? parametros = null)
        =>await _conexion.ExecuteAsync (nombreSP, param: parametros,
                            commandType: CommandType.StoredProcedure);

    public IEnumerable<T> EjecutarSPConReturnDeTipoLista<T>(string nombreSP, DynamicParameters? parametros = null)
        => _conexion.Query<T>(nombreSP, param: parametros , commandType: CommandType.StoredProcedure);
}
    