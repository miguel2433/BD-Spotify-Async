using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Spotify.ReposDapper;
using Spotify.Core;
using System.Threading.Tasks;

namespace BD_Sporify._MVC.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private readonly IRepoUsuarioAsync repoUsuario;
    private readonly IRepoNacionalidadAsync repoNacionalidad;
    public UsuarioController(ILogger<UsuarioController> logger, IRepoUsuarioAsync repoUsuario, IRepoNacionalidadAsync repoNacionalidad)
    {
        _logger = logger;
        this.repoUsuario = repoUsuario;
        this.repoNacionalidad = repoNacionalidad;
    }

    public IActionResult Login() => View();
    public async Task<IActionResult> Registrar()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home"); 
        }
        var vm = new CrearUsuarioModel
        {
            Nacionalidades = await repoNacionalidad.Obtener()
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Registrar(CrearUsuarioModel model)
    {
        var nacionalidadSeleccionada = await repoNacionalidad.DetalleDe(model.NacionalidadId);

        Usuario usuario1 = new Usuario()
        {
            NombreUsuario = model.NombreUsuario,
            Email = model.Email,
            Contrasenia = model.Contrasenia,
            nacionalidad = nacionalidadSeleccionada
        };
        await repoUsuario.Alta(usuario1);

        // Loguear automáticamente
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario1.idUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario1.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario1.Email),
            new Claim(ClaimTypes.Role, usuario1.Rol.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Login(Usuario usuario)
    {
        var usuario1 = await repoUsuario.LoginUsuarioAsync(usuario.Email , usuario.Contrasenia);
        if (usuario1 == null)
        {
            return View(usuario);
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario1.idUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario1.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario1.Email),
            new Claim(ClaimTypes.Role, usuario1.Rol.ToString())
        };



        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }
        return RedirectToAction("Index", "Home"); 

    }
}
