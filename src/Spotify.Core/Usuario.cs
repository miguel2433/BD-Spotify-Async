namespace Spotify.Core
{
    public enum RolUsuario
    {
        Usuario, 
        Admin
    }

    public class Usuario
    {
        public uint idUsuario { get; set; }
        public required string NombreUsuario { get; set; }
        public required string Email { get; set; }
        public required string Contrasenia { get; set; }
        public required Nacionalidad nacionalidad { get; set; }

        public RolUsuario Rol { get; set; } = RolUsuario.Usuario;
    }
}