using System.Threading.Tasks;

namespace Spotify.ReposDapper;

public class RepoUsuarioAsync : RepoGenerico, IRepoUsuarioAsync
{
    public RepoUsuarioAsync(IDbConnection conexion) 
        : base(conexion) {}

    public async Task<uint> Alta(Usuario usuario)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@unidUsuario", direction: ParameterDirection.Output);
        parametros.Add("@unNombreUsuario", usuario.NombreUsuario);
        parametros.Add("@unaContrasenia", usuario.Contrasenia);
        parametros.Add("@unEmail", usuario.Gmail);
        parametros.Add("@unidNacionalidad", usuario.nacionalidad.idNacionalidad);

        await _conexion.ExecuteAsync("altaUsuario", parametros, commandType: CommandType.StoredProcedure);

        usuario.idUsuario = parametros.Get<uint>("@unidUsuario");
        return usuario.idUsuario;
    }

    public async Task<Usuario?> DetalleDe(uint idUsuario)
    {
        string BuscarUsuario = @"SELECT * FROM Usuario WHERE idUsuario = @idUsuario";

        // Ejecutar la consulta y obtener el primer resultado o 'null' si no existe.
        var usuario = await _conexion.QueryFirstOrDefaultAsync<Usuario>(BuscarUsuario, new { idUsuario });

        return usuario;
    }

    public IList<Usuario> Obtener() => EjecutarSPConReturnDeTipoLista<Usuario>("ObtenerUsuarios").ToList();
}
