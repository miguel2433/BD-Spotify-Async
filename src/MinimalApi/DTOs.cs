using Spotify.Core;


namespace DTOs;


public record struct NacionalidadDTO(uint idNacionalidad, string Pais);

public record struct NacionalidadAltaDTO(string Pais);

public record struct AlbumDetalleDTO(uint idAlbum, string Titulo, DateTime FechaLanzamiento, Artista artista);
