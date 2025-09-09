using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Threading.Tasks;
using Spotify.Core;

namespace SpotifyMVC.Models
{
    public class AlbunesViewModel
    {
        public List<Album> albums { get; set; } = new();

    }
}