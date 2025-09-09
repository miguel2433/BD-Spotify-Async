using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SpotifyMVC.Views.Album
{
    public class CrearAlbum : PageModel
    {
        private readonly ILogger<CrearAlbum> _logger;

        public CrearAlbum(ILogger<CrearAlbum> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}