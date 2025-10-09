using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class DetalleAlbumViewModel
    {
        public Album album { get; set; }
        public Artista artista { get; set; }
        public List<Cancion> canciones { get; set; }
    }
}