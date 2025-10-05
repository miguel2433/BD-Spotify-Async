USE 5to_Spotify;

-- Tabla Artista
 CALL altaArtista('some.jpg','TripleT', 'Miguel', 'Verduguez', @idArtistaTripleT);
 CALL altaArtista('some.jpg','El Chapo', 'Josu', 'Duran', @idArtistaElChapo);
 CALL altaArtista('some.jpg','La Maquinaria', 'Rene', 'Terrazas', @idArtistalaMaquinaria);
 CALL altaArtista('some.jpg','El Renacido', 'Leonardo', 'Cheng', @idArtistaElRenacido);

-- Tabla Album
-- Álbumes con fechas de ejemplo
CALL altaAlbum(@idAlbumEcos,'Ecos del Pasado',@idArtistaElRenacido,'2018-05-20','some.jpg');
CALL altaAlbum(@idAlbumSuenios,'Sueños de Verano',@idArtistalaMaquinaria,'2019-08-10','some.jpg');
CALL altaAlbum(@idAlbumCaminos,'Caminos Cruzados',@idArtistaElChapo,'2020-03-15','some.jpg');
CALL altaAlbum(@idAlbumLuz,'Luz y Sombra',@idArtistaTripleT,'2021-11-05','some.jpg');

-- Tabla Nacionalidad
CALL altaNacionalidad ('Argentina', @idNacionalidadArgentina);
CALL altaNacionalidad ('Brasil', @idNacionalidadBrasil );
CALL altaNacionalidad ('U.R.S.S.', @idNacionalidadURSS);
CALL altaNacionalidad ('Bolivia', @idNacionalidadBolivia);

-- Tabla Usuario
CALL altaUsuario ("Miguel", "miguelito@gmail.com", "Deadpool3saliomal", 1, @idUsuarioMiguel);
CALL altaUsuario ("Josu","josu@gmail.com","OMORIoyasumi",2,@idUsuarioJosu);
CALL altaUsuario ("Rene","rene@gmail.com","Totoro",3,@idUsuarioRene);
CALL altaUsuario ("Cheng","chengleonardo@gmail.com","capitalismonofunciona",4,@idUsuarioCheng);

-- Tabla Genero
CALL altaGenero('Hip-hop/Rap', @idGeneroHipHop);
CALL altaGenero('Jazz', @idGeneroJazz);
CALL altaGenero('Reggae', @idGeneroReggae);
CALL altaGenero('Ranchera', @idGeneroRanchera);

-- Tabla Cancion
CALL altaCancion(@idCancionOver, 'some.jpg','Its Over, Isnt It', '00:02:17', @idAlbumSuenios, @idArtistaElRenacido, @idGeneroHipHop);
CALL altaCancion(@idCancionRene, 'some.jpg','René', '00:07:41', @idAlbumCaminos, @idArtistaElChapo, @idGeneroJazz);
CALL altaCancion(@idCancionEstrella, 'some.jpg','Como Estrella', '00:03:40', @idAlbumLuz, @idArtistalaMaquinaria, @idGeneroReggae);
CALL altaCancion(@idCancionCelos,'some.jpg', 'Estos Celos', '00:03:10', @idAlbumEcos, @idArtistaTripleT, @idGeneroRanchera);

-- Tabla Historial Reproduccion
CALL altaHistorial_reproduccion(@idHistorialMiguel, @idUsuarioMiguel, @idCancionOver, '2024-07-01 10:00:00');
CALL altaHistorial_reproduccion(@idHistorialJosu, @idUsuarioJosu, @idCancionRene, '2024-07-01 11:00:00');
CALL altaHistorial_reproduccion(@idHistorialRene, @idUsuarioRene, @idCancionEstrella, '2024-07-01 12:00:00');
CALL altaHistorial_reproduccion(@idHistorialCheng, @idUsuarioCheng, @idCancionCelos, '2024-07-01 13:00:00');

-- Tabla Playlist
CALL altaPlaylist('some.jpg','Éxitos de Rock', @idUsuarioMiguel,@idPlaylistRock);
CALL altaPlaylist('some.jpg','Clásicos del Pop', @idUsuarioJosu,@idPlaylistPop);
CALL altaPlaylist('some.jpg','Vibras de Jazz', @idUsuarioRene,@idPlaylistJazz);
CALL altaPlaylist('some.jpg','Ritmos Chill', @idUsuarioCheng, @idPlaylistChill);

-- Tabla TipoSuscripcion
CALL altaTipoSuscripcion(@idSuscripcionMensual,1,8,"Mensual");
CALL altaTipoSuscripcion(@idSuscripcionBimestral,2,12,"Bimestral");
CALL altaTipoSuscripcion(@idSuscripcionTrimestral,3,15,"Trimestral");
CALL altaTipoSuscripcion(@idSuscripcionCuatrimestral,4,20,"Cuatrimestral");

-- Tabla Cancion_Playlist

CALL altaPlaylistCancion(@idCancionOver, @idPlaylistRock);
CALL altaPlaylistCancion(@idCancionRene, @idPlaylistPop);
CALL altaPlaylistCancion(@idCancionEstrella, @idPlaylistJazz);
CALL altaPlaylistCancion(@idCancionCelos, @idPlaylistChill);



-- Tabla Suscripcion_Usuario
INSERT INTO Suscripcion (idUsuario,idSuscripcion,idTipoSuscripcion,FechaInicio)
 VALUES
        (1,1,@idSuscripcionMensual,"2024-5-3"),
        (2,2,@idSuscripcionBimestral,"2024-7-23"),
        (3,3,@idSuscripcionTrimestral,"2024-7-23");
