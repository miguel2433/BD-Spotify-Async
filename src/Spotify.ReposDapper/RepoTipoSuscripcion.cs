using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoTipoSuscripcion : RepoGenerico, IRepoTipoSuscripcion
{
    public RepoTipoSuscripcion(IDbConnection conexion) 
        : base(conexion) {}

    public async Task<uint> Alta(TipoSuscripcion tipoSuscripcion)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidTipoSuscripcion", direction: ParameterDirection.Output);
        parametros.Add("@unCosto", tipoSuscripcion.Costo);
        parametros.Add("@unaDuracion", tipoSuscripcion.Duracion);
        parametros.Add("@UntipoSuscripcion", tipoSuscripcion.Tipo);

        await _conexion.ExecuteAsync("altaTipoSuscripcion", parametros, commandType: CommandType.StoredProcedure);

        tipoSuscripcion.IdTipoSuscripcion = parametros.Get<uint>("@unidTipoSuscripcion");

        return tipoSuscripcion.IdTipoSuscripcion;
    }

    public async Task<TipoSuscripcion> DetalleDe(uint idTipoSuscripcion)
    {
        var BuscarTipoSuscripcionPorId = @"
        Select * 
        FROM TipoSuscripcion
        Where idTipoSuscripcion = @idTipoSuscripcion
        ";
        
        var TipoSuscripcion = await _conexion.QueryFirstOrDefaultAsync<TipoSuscripcion>(BuscarTipoSuscripcionPorId, new {idTipoSuscripcion});

        return TipoSuscripcion;
    }

    public IList<TipoSuscripcion> Obtener() => EjecutarSPConReturnDeTipoLista<TipoSuscripcion>("ObtenerTipoSuscripciones").ToList();

}