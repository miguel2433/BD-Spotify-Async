USE 5to_Spotify;

-- Tabla Artista
 CALL altaArtista('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','TripleT', 'Miguel', 'Verduguez', @idArtistaTripleT);
 CALL altaArtista('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','El Chapo', 'Josu', 'Duran', @idArtistaElChapo);
 CALL altaArtista('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','La Maquinaria', 'Rene', 'Terrazas', @idArtistalaMaquinaria);
 CALL altaArtista('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','El Renacido', 'Leonardo', 'Cheng', @idArtistaElRenacido);

-- Tabla Album
-- Álbumes con fechas de ejemplo
CALL altaAlbum(@idAlbumEcos,'Ecos del Pasado',@idArtistaElRenacido,'2018-05-20','1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png');
CALL altaAlbum(@idAlbumSuenios,'Sueños de Verano',@idArtistalaMaquinaria,'2019-08-10','1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png');
CALL altaAlbum(@idAlbumCaminos,'Caminos Cruzados',@idArtistaElChapo,'2020-03-15','1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png');
CALL altaAlbum(@idAlbumLuz,'Luz y Sombra',@idArtistaTripleT,'2021-11-05','1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png');

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
CALL altaCancion(@idCancionOver, '1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Its Over, Isnt It', '00:02:17', @idAlbumSuenios, @idArtistaElRenacido, @idGeneroHipHop);
CALL altaCancion(@idCancionRene, '1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','René', '00:07:41', @idAlbumCaminos, @idArtistaElChapo, @idGeneroJazz);
CALL altaCancion(@idCancionEstrella, '1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Como Estrella', '00:03:40', @idAlbumLuz, @idArtistalaMaquinaria, @idGeneroReggae);
CALL altaCancion(@idCancionCelos,'1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png', 'Estos Celos', '00:03:10', @idAlbumEcos, @idArtistaTripleT, @idGeneroRanchera);

-- Tabla Historial Reproduccion
CALL altaHistorial_reproduccion(@idHistorialMiguel, @idUsuarioMiguel, @idCancionOver, '2024-07-01 10:00:00');
CALL altaHistorial_reproduccion(@idHistorialJosu, @idUsuarioJosu, @idCancionRene, '2024-07-01 11:00:00');
CALL altaHistorial_reproduccion(@idHistorialRene, @idUsuarioRene, @idCancionEstrella, '2024-07-01 12:00:00');
CALL altaHistorial_reproduccion(@idHistorialCheng, @idUsuarioCheng, @idCancionCelos, '2024-07-01 13:00:00');

-- Tabla Playlist
CALL altaPlaylist('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Éxitos de Rock', @idUsuarioMiguel,@idPlaylistRock);
CALL altaPlaylist('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Clásicos del Pop', @idUsuarioJosu,@idPlaylistPop);
CALL altaPlaylist('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Vibras de Jazz', @idUsuarioRene,@idPlaylistJazz);
CALL altaPlaylist('1f2b40ca-7d4f-4d01-91df-68865ab488c7_Screenshot_1.png','Ritmos Chill', @idUsuarioCheng, @idPlaylistChill);

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
