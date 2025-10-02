using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class AlbumViewModel
    {
        public List<Artista> artistas { get; set; } = new();
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; } = DateTime.Now;
        public uint ArtistaId { get; set; }

    }
}