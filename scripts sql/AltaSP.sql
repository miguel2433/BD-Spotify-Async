USE 5to_Spotify;

-- Tabla Artista
DELIMITER $$
DROP PROCEDURE IF EXISTS altaArtista $$
CREATE PROCEDURE altaArtista (unImageUrl VARCHAR(255),unNombreArtistico VARCHAR(35), unNombre VARCHAR(45), unApellido VARCHAR(45), out unidArtista INT UNSIGNED)
BEGIN 
    INSERT INTO Artista(ImageUrl,NombreArtistico,Nombre,Apellido)
   	    VALUES(unImageUrl,unNombreArtistico,unNombre,unApellido);
    
	SET unidArtista = last_insert_id();
END $$

-- Tabla Album
DELIMITER $$
DROP PROCEDURE IF EXISTS altaAlbum $$
CREATE PROCEDURE altaAlbum (OUT unidAlbum INT UNSIGNED,
    unTitulo VARCHAR(45),
    unidArtista INT UNSIGNED,
	unFechaLanzamiento DATE,
	unImageUrl VARCHAR(255))
BEGIN 
	INSERT INTO Album (Titulo,fechaLanzamiento,idArtista,ImageUrl)
		VALUES(unTitulo,unFechaLanzamiento,unidArtista,unImageUrl );

	SET unidAlbum = last_insert_id();
END$$


-- Tabla Nacionalidad
DELIMITER $$
DROP PROCEDURE IF EXISTS altaNacionalidad $$
CREATE PROCEDURE altaNacionalidad (unPais VARCHAR(45), out unidNacionalidad INT UNSIGNED)
BEGIN
    INSERT INTO Nacionalidad (Pais)
        VALUES(unPais);
    
    SET unidNacionalidad = last_insert_id();
END $$

-- Tabla Usuario
DELIMITER $$
DROP PROCEDURE IF EXISTS altaUsuario $$
CREATE PROCEDURE altaUsuario (unNombreUsuario VARCHAR(45), unEmail VARCHAR(45), unaContrasenia VARCHAR(64), unidNacionalidad INT UNSIGNED, out unidUsuario INT UNSIGNED)
BEGIN
    INSERT INTO Usuario(NombreUsuario,Email,Contrasenia,idNacionalidad)
   	    VALUES(unNombreUsuario,unEmail,SHA2(unaContrasenia, 256),unidNacionalidad);
    
	SET unidUsuario = last_insert_id();
END $$

-- Tabla Genero
DELIMITER $$
DROP PROCEDURE IF EXISTS altaGenero $$
CREATE PROCEDURE altaGenero (unGenero VARCHAR(45), out unidGenero tinyint unsigned)
BEGIN
    INSERT INTO Genero (Genero)
        VALUES(unGenero);
    
	SET unidGenero = last_insert_id();
END$$

-- Tabla Cancion
DELIMITER $$

DROP PROCEDURE IF EXISTS altaCancion $$
CREATE PROCEDURE altaCancion (
    OUT unidCancion INT UNSIGNED,
    IN unImageUrl VARCHAR(255),
    IN unAudioUrl VARCHAR(255),
    IN unTitulo VARCHAR(45),
    IN unDuration TIME,
    IN unidAlbum INT UNSIGNED,
    IN unidArtista INT UNSIGNED,
    IN unidGenero TINYINT UNSIGNED
)
BEGIN 
    INSERT INTO Cancion (ImageUrl, AudioUrl, Titulo, duration, idAlbum, idArtista, idGenero)
    VALUES (unImageUrl, unAudioUrl, unTitulo, unDuration, unidAlbum, unidArtista, unidGenero);

    SET unidCancion = LAST_INSERT_ID();
END $$

-- Tabla Historial Reproduccion
DELIMITER $$
DROP PROCEDURE IF EXISTS altaHistorial_reproduccion $$
CREATE PROCEDURE altaHistorial_reproduccion (OUT unidHistorial INT UNSIGNED, unidUsuario INT UNSIGNED, unidCancion INT UNSIGNED, unFechaReproduccion DATETIME)
BEGIN 
	INSERT INTO HistorialReproduccion (idUsuario,idCancion,FechaReproduccion)
		VALUES(unidUsuario,unidCancion,unFechaReproduccion);
	
	SET unidHistorial = last_insert_id();
END $$

-- Tabla Playlist
DELIMITER $$
DROP PROCEDURE IF EXISTS altaPlaylist $$
CREATE PROCEDURE altaPlaylist (unImageUrl VARCHAR(255),unNombre VARCHAR(20), unidUsuario INT UNSIGNED, out unidPlaylist INT UNSIGNED)
BEGIN
	INSERT INTO Playlist(ImageUrl,Nombre,idUsuario)
	    VALUES(unImageUrl,unNombre,unidUsuario);
	
    SET unidPlaylist = last_insert_id();
END $$

-- Tabla TipoSuscripcion
DELIMITER $$
DROP PROCEDURE IF EXISTS altaTipoSuscripcion $$
CREATE PROCEDURE altaTipoSuscripcion (OUT unidTipoSuscripcion INT UNSIGNED, unaDuracion TINYINT UNSIGNED, unCosto TINYINT UNSIGNED, UntipoSuscripcion VARCHAR(45))
BEGIN
	INSERT INTO TipoSuscripcion (Duracion,Costo,Tipo)
		VALUES(unaDuracion,unCosto,UntipoSuscripcion);
	
    SET unidTipoSuscripcion = last_insert_id();
END $$


--	Registro Suscripcion

DELIMITER $$
DROP PROCEDURE IF EXISTS altaRegistroSuscripcion $$
CREATE PROCEDURE altaRegistroSuscripcion (out unidSuscripcion INT UNSIGNED,unIdUsuario INT UNSIGNED,unidTipoSuscripcion INT UNSIGNED)
BEGIN
	INSERT INTO Suscripcion (idUsuario,idTipoSuscripcion,FechaInicio)
		VALUES (unIdUsuario,unidTipoSuscripcion,CURDATE());

	set unidSuscripcion = last_insert_id();
END $$

DELIMITER $$
DROP PROCEDURE IF EXISTS altaPlaylistCancion $$
CREATE PROCEDURE altaPlaylistCancion (unidCancion INT UNSIGNED,unidPlaylist INT UNSIGNED)
BEGIN 
	INSERT INTO Cancion_Playlist(idCancion, idPlaylist)
	VALUES (unidCancion, unidPlaylist);
END$$


DELIMITER $$
DROP PROCEDURE IF EXISTS MatcheoCancion $$
CREATE PROCEDURE MatcheoCancion(InputCancion VARCHAR(45))
BEGIN
	SELECT Titulo
	FROM Cancion
	WHERE MATCH(Titulo) AGAINST(CONCAT(InputCancion, "*") IN BOOLEAN MODE);
END$$
