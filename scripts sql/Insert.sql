USE 5to_Spotify;

-- Tabla Artista
 CALL altaArtista('ddbf830c-be5d-4f23-90fb-c0348486206a_images.jpeg','TripleT', 'Miguel', 'Verduguez', @idArtistaTripleT);
 CALL altaArtista('fd8a941b-6b05-4430-9391-914002a52d61_emanen.jpeg','El Chapo', 'Josu', 'Duran', @idArtistaElChapo);
 CALL altaArtista('6a264497-86e4-429b-ae0b-f74bce50ea88_descarga.jpeg','La Maquinaria', 'Rene', 'Terrazas', @idArtistalaMaquinaria);
 CALL altaArtista('5e00475f-d13f-4773-85cc-a2bd00f1be4d_johan.jpeg','El Renacido', 'Leonardo', 'Cheng', @idArtistaElRenacido);

-- Tabla Album
-- Álbumes con fechas de ejemplo
CALL altaAlbum(@idAlbumEcos,'Ecos del Pasado',@idArtistaElRenacido,'2018-05-20','e5763e9c-8561-49ba-98e1-7b0035b49ee8_album2.jpeg');
CALL altaAlbum(@idAlbumSuenios,'Sueños de Verano',@idArtistalaMaquinaria,'2019-08-10','2ac7a8ca-e11e-42c8-86f6-74e31e3834bf_album3.jpeg');
CALL altaAlbum(@idAlbumCaminos,'Caminos Cruzados',@idArtistaElChapo,'2020-03-15','b29e6384-cc69-4c08-b733-7d3439e5a1ae_Album1.jpeg');
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

INSERT INTO Usuario (NombreUsuario, Email, Contrasenia, idNacionalidad, Rol)
VALUES ("SoyAdmin", "admin@gmail.com", SHA2("123", 256), 2, 'Admin');


-- Tabla Genero
CALL altaGenero('Hip-hop/Rap', @idGeneroHipHop);
CALL altaGenero('Jazz', @idGeneroJazz);
CALL altaGenero('Reggae', @idGeneroReggae);
CALL altaGenero('Ranchera', @idGeneroRanchera);

-- Tabla Cancion
CALL altaCancion(@idCancionOver, '247ea913-3b7e-4b30-ae31-1968ee182d99_cancion3.jpeg','7faf1d0e-c89a-46b7-8ba4-950313d6679d_Pimpinela — A Esa [Letra].mp4','Its Over, Isnt It', '00:03:08', @idAlbumSuenios, @idArtistaElRenacido, @idGeneroHipHop);
CALL altaCancion(@idCancionRene, '7240c38b-ef74-48a9-bfa9-4c0c6f588d26_Cancion2.jpeg','5d776607-d39e-44ec-9fb1-487eae76850a_Kiss - I Was Made For Lovin You.mp4','René', '00:03:58', @idAlbumCaminos, @idArtistaElChapo, @idGeneroJazz);
CALL altaCancion(@idCancionEstrella, '408f992b-392e-4d10-b290-2bec145e1bd9_cancion4.jpeg','2e57d23b-fcef-491a-9f7c-621a7ad962d9_Radiohead - No Surprises.mp4','Como Estrella', '00:03:47', @idAlbumLuz, @idArtistalaMaquinaria, @idGeneroReggae);
CALL altaCancion(@idCancionCelos,'fbaa7644-d6e7-4fa8-8e24-53a5bcc9f988_Cancion1.jpeg', 'd907f8ad-6a27-46f2-89a6-a87795dd3c9f_Queen – Bohemian Rhapsody (Official Video Remastered).mp4','Estos Celos', '00:05:59', @idAlbumEcos, @idArtistaTripleT, @idGeneroRanchera);

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
