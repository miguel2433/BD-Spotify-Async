using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class AgregarCancionAPlaylistViewModel
    {
        public Playlist playlist { get; set; }
        public List<Cancion> TodasLasCanciones { get; set; }
        public uint IdCancionSeleccionada { get; set; }
    }
}