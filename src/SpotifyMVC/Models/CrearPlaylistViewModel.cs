using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class CrearPlaylistViewModel
    {
        public Playlist playlist { get; set; }
    
        public IFormFile ImageUrl { get; set; }
}
}
