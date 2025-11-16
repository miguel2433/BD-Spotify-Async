using Microsoft.AspNetCore.Mvc;
using Spotify.Core;
using Spotify.ReposDapper; 
using SpotifyMVC.Models;
using Spotify.Core.Persistencia;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class AsidePlaylistsViewComponent : ViewComponent
{
    private readonly IRepoPlaylistAsync _repo;

    public AsidePlaylistsViewComponent(IRepoPlaylistAsync repo)
    {
        _repo = repo;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Sacar id del usuario desde las claims
        var userIdClaim = HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
            return View(new List<Playlist>()); // usuario no logueado

        uint idUsuario = uint.Parse(userIdClaim);

    
        var playlists = await _repo.PlaylistsDelUsuario(idUsuario);

        return View(playlists);
    }
}