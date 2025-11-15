using Microsoft.AspNetCore.Mvc.Rendering;
using Spotify.Core;

namespace SpotifyMVC.Models;

public class CrearUsuarioModel
{
    public List<Nacionalidad> Nacionalidades { get; set; } = new();
    public string NombreUsuario {get;set;}
    public string Email { get; set; }
    public string Contrasenia { get; set; } 
    public uint NacionalidadId { get; set; }

}
