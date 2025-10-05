using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class CrearArtistaViewModel
    {
        public Artista artista { get; set; }
    
        public IFormFile ImageUrl { get; set; }
}
}
