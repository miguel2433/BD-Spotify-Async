using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class HomeViewModel
    {
        public List<Artista> artistas {get; set;} = new();
        public List<Album> albunes {get;set;} = new();
        public List<Cancion> canciones {get;set;} = new();
    }
}