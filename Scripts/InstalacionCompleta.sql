-- ====================================================================
-- TechSystem - SCRIPT UNICO DE INSTALACION
-- Base de datos: TechSystemDB
--
-- Este script hace TODO en un solo paso:
--   1. Crea la base de datos TechSystemDB (si no existe)
--   2. Crea las 6 tablas con sus relaciones y datos de prueba
--   3. Crea los 42 procedimientos almacenados
--
-- COMO USARLO:
-- 1. Abra SQL Server Management Studio (SSMS) y conectese a
--    DESKTOP-P6SNJH4\SQLEXPRESS O su instancia por defecto
-- 2. Ejecute este script completo.
--
-- USUARIO DE PRUEBA: randall@uhispano.ac.cr   Clave: 123
--
-- NOTA: este script RECREA la base de datos (borra las tablas y las
-- vuelve a crear con datos de prueba). No lo ejecute sobre una base
-- de datos que quiera conservar.
-- ====================================================================
-- Paso 1: Crear la base de datos (si no existe)
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TechSystemDB')
BEGIN
    CREATE DATABASE TechSystemDB;
END
GO

USE TechSystemDB;
GO

-- =============================================
-- Paso 2: Eliminar tablas si ya existen (para recrear sin problemas)
-- =============================================
IF OBJECT_ID('dbo.Asignaciones', 'U') IS NOT NULL DROP TABLE dbo.Asignaciones;
IF OBJECT_ID('dbo.DetallesReparacion', 'U') IS NOT NULL DROP TABLE dbo.DetallesReparacion;
IF OBJECT_ID('dbo.Reparaciones', 'U') IS NOT NULL DROP TABLE dbo.Reparaciones;
IF OBJECT_ID('dbo.Tecnicos', 'U') IS NOT NULL DROP TABLE dbo.Tecnicos;
IF OBJECT_ID('dbo.Equipos', 'U') IS NOT NULL DROP TABLE dbo.Equipos;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

-- =============================================
-- Paso 3: Crear la tabla Usuarios
-- Relacion: 1 usuario puede tener muchos equipos (1:N)
-- La columna Clave guarda la clave encriptada (hash de 64 caracteres)
-- =============================================
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    CorreoElectronico NVARCHAR(150) NOT NULL,
    Telefono NVARCHAR(20) NULL,
    Clave NVARCHAR(64) NULL
);
GO

-- =============================================
-- Paso 4: Crear la tabla Equipos
-- Relacion: FK hacia Usuarios (un equipo pertenece a un solo usuario)
-- =============================================
CREATE TABLE Equipos (
    EquipoID INT IDENTITY(1,1) PRIMARY KEY,
    TipoEquipo NVARCHAR(50) NOT NULL,
    Modelo NVARCHAR(100) NOT NULL,
    UsuarioID INT NOT NULL,
    CONSTRAINT FK_Equipos_Usuarios FOREIGN KEY (UsuarioID)
        REFERENCES Usuarios(UsuarioID)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- =============================================
-- Paso 5: Crear la tabla Tecnicos
-- =============================================
CREATE TABLE Tecnicos (
    TecnicoID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100) NOT NULL
);
GO

-- =============================================
-- Paso 6: Crear la tabla Reparaciones
-- Relacion: FK hacia Equipos (una reparacion es de un solo equipo)
-- =============================================
CREATE TABLE Reparaciones (
    ReparacionID INT IDENTITY(1,1) PRIMARY KEY,
    EquipoID INT NOT NULL,
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Pendiente',
    CONSTRAINT FK_Reparaciones_Equipos FOREIGN KEY (EquipoID)
        REFERENCES Equipos(EquipoID)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- =============================================
-- Paso 7: Crear la tabla DetallesReparacion
-- Relacion: FK hacia Reparaciones (una reparacion puede tener muchos detalles)
-- =============================================
CREATE TABLE DetallesReparacion (
    DetalleID INT IDENTITY(1,1) PRIMARY KEY,
    ReparacionID INT NOT NULL,
    Descripcion NVARCHAR(500) NOT NULL,
    FechaInicio DATETIME NULL,
    FechaFin DATETIME NULL,
    CONSTRAINT FK_DetallesReparacion_Reparaciones FOREIGN KEY (ReparacionID)
        REFERENCES Reparaciones(ReparacionID)
        ON DELETE CASCADE ON UPDATE NO ACTION
);
GO

-- =============================================
-- Paso 8: Crear la tabla Asignaciones (tabla intermedia N:M)
-- Relacion: FK hacia Reparaciones y Tecnicos
-- Una reparacion puede tener varios tecnicos, y un tecnico varias reparaciones
-- =============================================
CREATE TABLE Asignaciones (
    AsignacionID INT IDENTITY(1,1) PRIMARY KEY,
    ReparacionID INT NOT NULL,
    TecnicoID INT NOT NULL,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Asignaciones_Reparaciones FOREIGN KEY (ReparacionID)
        REFERENCES Reparaciones(ReparacionID)
        ON DELETE CASCADE ON UPDATE NO ACTION,
    CONSTRAINT FK_Asignaciones_Tecnicos FOREIGN KEY (TecnicoID)
        REFERENCES Tecnicos(TecnicoID)
        ON DELETE NO ACTION ON UPDATE NO ACTION
);
GO

-- =============================================
-- Paso 9: Insertar datos de prueba
-- =============================================

-- Usuarios (la clave de todos es: 123, se guarda encriptada con SHA256)
-- HASHBYTES genera el mismo codigo que usa el sistema para encriptar
INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono, Clave) VALUES
('Randall Sanchez', 'randall@uhispano.ac.cr', '8888-1111', LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123'), 2))),
('Maria Rodriguez', 'maria@uhispano.ac.cr', '8888-2222', LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123'), 2))),
('Carlos Jimenez', 'carlos@uhispano.ac.cr', '8888-3333', LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123'), 2)));
GO

-- Equipos
INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID) VALUES
('Laptop', 'Dell Latitude 5520', 1),
('Desktop', 'HP ProDesk 600 G6', 2),
('Impresora', 'Epson EcoTank L3250', 1),
('Monitor', 'Samsung 24" FHD', 3);
GO

-- Tecnicos
INSERT INTO Tecnicos (Nombre, Especialidad) VALUES
('Juan Perez', 'Hardware y Redes'),
('Ana Lopez', 'Software y Sistemas Operativos'),
('Luis Mora', 'Impresoras y Perifericos');
GO

-- Reparaciones
INSERT INTO Reparaciones (EquipoID, FechaSolicitud, Estado) VALUES
(1, '2026-07-15 09:30:00', 'Pendiente'),
(2, '2026-07-16 10:00:00', 'En Proceso'),
(3, '2026-07-17 08:45:00', 'Completada'),
(1, '2026-07-18 14:20:00', 'Pendiente');
GO

-- DetallesReparacion
INSERT INTO DetallesReparacion (ReparacionID, Descripcion, FechaInicio, FechaFin) VALUES
(2, 'Diagnostico inicial del sistema operativo', '2026-07-16 10:15:00', NULL),
(2, 'Reinstalacion de drivers y actualizaciones', '2026-07-16 11:00:00', NULL),
(3, 'Cambio de cartuchos y limpieza de cabezales', '2026-07-17 09:00:00', '2026-07-17 10:30:00');
GO

-- Asignaciones
INSERT INTO Asignaciones (ReparacionID, TecnicoID, FechaAsignacion) VALUES
(1, 1, '2026-07-15 09:45:00'),
(2, 2, '2026-07-16 10:05:00'),
(3, 3, '2026-07-17 08:50:00'),
(4, 1, '2026-07-18 14:30:00');
GO

PRINT 'Base de datos TechSystemDB creada exitosamente con todas las tablas, relaciones y datos de prueba.';
GO
USE TechSystemDB;
GO

-- ================= USUARIOS =================

IF OBJECT_ID('sp_Usuarios_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Listar;
GO

CREATE PROCEDURE sp_Usuarios_Listar
AS
BEGIN
    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM Usuarios
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Usuarios_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Buscar;
GO

CREATE PROCEDURE sp_Usuarios_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM Usuarios
    WHERE Nombre LIKE '%' + @texto + '%'
       OR CorreoElectronico LIKE '%' + @texto + '%'
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Usuarios_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_ObtenerPorId;
GO

CREATE PROCEDURE sp_Usuarios_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT UsuarioID, Nombre, CorreoElectronico, Telefono
    FROM Usuarios
    WHERE UsuarioID = @id;
END
GO

IF OBJECT_ID('sp_Usuarios_Login', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Login;
GO

CREATE PROCEDURE sp_Usuarios_Login
    @correo NVARCHAR(100),
    @clave NVARCHAR(64)
AS
BEGIN
    SELECT UsuarioID, Nombre
    FROM Usuarios
    WHERE CorreoElectronico = @correo AND Clave = @clave;
END
GO

IF OBJECT_ID('sp_Usuarios_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Insertar;
GO

CREATE PROCEDURE sp_Usuarios_Insertar
    @nombre NVARCHAR(100),
    @correo NVARCHAR(100),
    @telefono NVARCHAR(20),
    @clave NVARCHAR(64)
AS
BEGIN
    INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono, Clave)
    VALUES (@nombre, @correo, @telefono, @clave);
END
GO

IF OBJECT_ID('sp_Usuarios_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Actualizar;
GO

CREATE PROCEDURE sp_Usuarios_Actualizar
    @id INT,
    @nombre NVARCHAR(100),
    @correo NVARCHAR(100),
    @telefono NVARCHAR(20),
    @clave NVARCHAR(64)
AS
BEGIN
    -- si la clave llega vacia, no se cambia (se queda la que tenia)
    IF @clave = ''
    BEGIN
        UPDATE Usuarios
        SET Nombre = @nombre,
            CorreoElectronico = @correo,
            Telefono = @telefono
        WHERE UsuarioID = @id;
    END
    ELSE
    BEGIN
        UPDATE Usuarios
        SET Nombre = @nombre,
            CorreoElectronico = @correo,
            Telefono = @telefono,
            Clave = @clave
        WHERE UsuarioID = @id;
    END
END
GO

IF OBJECT_ID('sp_Usuarios_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Usuarios_Eliminar;
GO

CREATE PROCEDURE sp_Usuarios_Eliminar
    @id INT
AS
BEGIN
    DELETE FROM Usuarios WHERE UsuarioID = @id;
END
GO

-- ================= EQUIPOS =================

IF OBJECT_ID('sp_Equipos_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_Listar;
GO

CREATE PROCEDURE sp_Equipos_Listar
AS
BEGIN
    SELECT e.EquipoID, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM Equipos e
    LEFT JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    ORDER BY e.TipoEquipo;
END
GO

IF OBJECT_ID('sp_Equipos_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_Buscar;
GO

CREATE PROCEDURE sp_Equipos_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT e.EquipoID, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM Equipos e
    LEFT JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    WHERE e.TipoEquipo LIKE '%' + @texto + '%'
       OR e.Modelo LIKE '%' + @texto + '%'
       OR u.Nombre LIKE '%' + @texto + '%'
    ORDER BY e.TipoEquipo;
END
GO

IF OBJECT_ID('sp_Equipos_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_ObtenerPorId;
GO

CREATE PROCEDURE sp_Equipos_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT EquipoID, TipoEquipo, Modelo, UsuarioID
    FROM Equipos
    WHERE EquipoID = @id;
END
GO

IF OBJECT_ID('sp_Equipos_ListarUsuarios', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_ListarUsuarios;
GO

CREATE PROCEDURE sp_Equipos_ListarUsuarios
AS
BEGIN
    SELECT UsuarioID, Nombre
    FROM Usuarios
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Equipos_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_Insertar;
GO

CREATE PROCEDURE sp_Equipos_Insertar
    @tipo NVARCHAR(100),
    @modelo NVARCHAR(100),
    @idUsuario INT
AS
BEGIN
    INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID)
    VALUES (@tipo, @modelo, @idUsuario);
END
GO

IF OBJECT_ID('sp_Equipos_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_Actualizar;
GO

CREATE PROCEDURE sp_Equipos_Actualizar
    @id INT,
    @tipo NVARCHAR(100),
    @modelo NVARCHAR(100),
    @idUsuario INT
AS
BEGIN
    UPDATE Equipos
    SET TipoEquipo = @tipo,
        Modelo = @modelo,
        UsuarioID = @idUsuario
    WHERE EquipoID = @id;
END
GO

IF OBJECT_ID('sp_Equipos_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Equipos_Eliminar;
GO

CREATE PROCEDURE sp_Equipos_Eliminar
    @id INT
AS
BEGIN
    DELETE FROM Equipos WHERE EquipoID = @id;
END
GO

-- ================= TECNICOS =================

IF OBJECT_ID('sp_Tecnicos_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_Listar;
GO

CREATE PROCEDURE sp_Tecnicos_Listar
AS
BEGIN
    SELECT TecnicoID, Nombre, Especialidad
    FROM Tecnicos
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Tecnicos_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_Buscar;
GO

CREATE PROCEDURE sp_Tecnicos_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT TecnicoID, Nombre, Especialidad
    FROM Tecnicos
    WHERE Nombre LIKE '%' + @texto + '%'
       OR Especialidad LIKE '%' + @texto + '%'
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Tecnicos_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_ObtenerPorId;
GO

CREATE PROCEDURE sp_Tecnicos_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT TecnicoID, Nombre, Especialidad
    FROM Tecnicos
    WHERE TecnicoID = @id;
END
GO

IF OBJECT_ID('sp_Tecnicos_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_Insertar;
GO

CREATE PROCEDURE sp_Tecnicos_Insertar
    @nombre NVARCHAR(100),
    @especialidad NVARCHAR(100)
AS
BEGIN
    INSERT INTO Tecnicos (Nombre, Especialidad)
    VALUES (@nombre, @especialidad);
END
GO

IF OBJECT_ID('sp_Tecnicos_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_Actualizar;
GO

CREATE PROCEDURE sp_Tecnicos_Actualizar
    @id INT,
    @nombre NVARCHAR(100),
    @especialidad NVARCHAR(100)
AS
BEGIN
    UPDATE Tecnicos
    SET Nombre = @nombre,
        Especialidad = @especialidad
    WHERE TecnicoID = @id;
END
GO

IF OBJECT_ID('sp_Tecnicos_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Tecnicos_Eliminar;
GO

CREATE PROCEDURE sp_Tecnicos_Eliminar
    @id INT
AS
BEGIN
    DELETE FROM Tecnicos WHERE TecnicoID = @id;
END
GO

-- ================= REPARACIONES =================

IF OBJECT_ID('sp_Reparaciones_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_Listar;
GO

CREATE PROCEDURE sp_Reparaciones_Listar
AS
BEGIN
    SELECT r.ReparacionID, r.FechaSolicitud, r.Estado,
           e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM Reparaciones r
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    ORDER BY r.FechaSolicitud DESC;
END
GO

IF OBJECT_ID('sp_Reparaciones_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_Buscar;
GO

CREATE PROCEDURE sp_Reparaciones_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT r.ReparacionID, r.FechaSolicitud, r.Estado,
           e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM Reparaciones r
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    WHERE r.Estado LIKE '%' + @texto + '%'
       OR e.TipoEquipo LIKE '%' + @texto + '%'
       OR e.Modelo LIKE '%' + @texto + '%'
       OR u.Nombre LIKE '%' + @texto + '%'
    ORDER BY r.FechaSolicitud DESC;
END
GO

IF OBJECT_ID('sp_Reparaciones_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_ObtenerPorId;
GO

CREATE PROCEDURE sp_Reparaciones_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT r.ReparacionID, r.EquipoID, r.FechaSolicitud, r.Estado
    FROM Reparaciones r
    WHERE r.ReparacionID = @id;
END
GO

IF OBJECT_ID('sp_Reparaciones_ListarEquipos', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_ListarEquipos;
GO

CREATE PROCEDURE sp_Reparaciones_ListarEquipos
AS
BEGIN
    SELECT e.EquipoID,
           e.TipoEquipo + ' - ' + e.Modelo + ' (' + u.Nombre + ')' AS NombreEquipo
    FROM Equipos e
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    ORDER BY e.TipoEquipo;
END
GO

IF OBJECT_ID('sp_Reparaciones_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_Insertar;
GO

CREATE PROCEDURE sp_Reparaciones_Insertar
    @equipoId INT,
    @estado NVARCHAR(50)
AS
BEGIN
    -- la fecha de solicitud se llena sola con la fecha actual (GETDATE)
    INSERT INTO Reparaciones (EquipoID, Estado)
    VALUES (@equipoId, @estado);
END
GO

IF OBJECT_ID('sp_Reparaciones_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_Actualizar;
GO

CREATE PROCEDURE sp_Reparaciones_Actualizar
    @id INT,
    @equipoId INT,
    @estado NVARCHAR(50)
AS
BEGIN
    UPDATE Reparaciones
    SET EquipoID = @equipoId,
        Estado = @estado
    WHERE ReparacionID = @id;
END
GO

IF OBJECT_ID('sp_Reparaciones_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Reparaciones_Eliminar;
GO

CREATE PROCEDURE sp_Reparaciones_Eliminar
    @id INT
AS
BEGIN
    -- los detalles y asignaciones de la reparacion se borran solos (CASCADE)
    DELETE FROM Reparaciones WHERE ReparacionID = @id;
END
GO

-- ================= DETALLES REPARACION =================

IF OBJECT_ID('sp_Detalles_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_Listar;
GO

CREATE PROCEDURE sp_Detalles_Listar
AS
BEGIN
    SELECT d.DetalleID, d.Descripcion, d.FechaInicio, d.FechaFin,
           r.ReparacionID, r.Estado, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM DetallesReparacion d
    JOIN Reparaciones r ON d.ReparacionID = r.ReparacionID
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    ORDER BY d.DetalleID;
END
GO

IF OBJECT_ID('sp_Detalles_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_Buscar;
GO

CREATE PROCEDURE sp_Detalles_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT d.DetalleID, d.Descripcion, d.FechaInicio, d.FechaFin,
           r.ReparacionID, r.Estado, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario
    FROM DetallesReparacion d
    JOIN Reparaciones r ON d.ReparacionID = r.ReparacionID
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    WHERE d.Descripcion LIKE '%' + @texto + '%'
       OR r.Estado LIKE '%' + @texto + '%'
       OR e.TipoEquipo LIKE '%' + @texto + '%'
       OR e.Modelo LIKE '%' + @texto + '%'
       OR u.Nombre LIKE '%' + @texto + '%'
    ORDER BY d.DetalleID;
END
GO

IF OBJECT_ID('sp_Detalles_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_ObtenerPorId;
GO

CREATE PROCEDURE sp_Detalles_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT d.DetalleID, d.ReparacionID, d.Descripcion, d.FechaInicio, d.FechaFin
    FROM DetallesReparacion d
    WHERE d.DetalleID = @id;
END
GO

IF OBJECT_ID('sp_Detalles_ListarReparaciones', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_ListarReparaciones;
GO

CREATE PROCEDURE sp_Detalles_ListarReparaciones
AS
BEGIN
    SELECT r.ReparacionID,
           'Reparacion #' + CONVERT(VARCHAR(10), r.ReparacionID) + ' - ' +
           e.TipoEquipo + ' ' + e.Modelo + ' (' + r.Estado + ')' AS NombreReparacion
    FROM Reparaciones r
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    ORDER BY r.ReparacionID DESC;
END
GO

IF OBJECT_ID('sp_Detalles_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_Insertar;
GO

CREATE PROCEDURE sp_Detalles_Insertar
    @reparacionId INT,
    @descripcion NVARCHAR(500),
    @fechaInicio DATETIME,
    @fechaFin DATETIME
AS
BEGIN
    INSERT INTO DetallesReparacion (ReparacionID, Descripcion, FechaInicio, FechaFin)
    VALUES (@reparacionId, @descripcion, @fechaInicio, @fechaFin);
END
GO

IF OBJECT_ID('sp_Detalles_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_Actualizar;
GO

CREATE PROCEDURE sp_Detalles_Actualizar
    @id INT,
    @reparacionId INT,
    @descripcion NVARCHAR(500),
    @fechaInicio DATETIME,
    @fechaFin DATETIME
AS
BEGIN
    UPDATE DetallesReparacion
    SET ReparacionID = @reparacionId,
        Descripcion = @descripcion,
        FechaInicio = @fechaInicio,
        FechaFin = @fechaFin
    WHERE DetalleID = @id;
END
GO

IF OBJECT_ID('sp_Detalles_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Detalles_Eliminar;
GO

CREATE PROCEDURE sp_Detalles_Eliminar
    @id INT
AS
BEGIN
    DELETE FROM DetallesReparacion WHERE DetalleID = @id;
END
GO

-- ================= ASIGNACIONES =================

IF OBJECT_ID('sp_Asignaciones_Listar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_Listar;
GO

CREATE PROCEDURE sp_Asignaciones_Listar
AS
BEGIN
    SELECT a.AsignacionID, a.FechaAsignacion,
           r.ReparacionID, r.Estado, e.TipoEquipo, e.Modelo,
           u.Nombre AS NombreUsuario,
           t.Nombre AS NombreTecnico, t.Especialidad
    FROM Asignaciones a
    JOIN Reparaciones r ON a.ReparacionID = r.ReparacionID
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    JOIN Tecnicos t ON a.TecnicoID = t.TecnicoID
    ORDER BY a.FechaAsignacion DESC;
END
GO

IF OBJECT_ID('sp_Asignaciones_Buscar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_Buscar;
GO

CREATE PROCEDURE sp_Asignaciones_Buscar
    @texto NVARCHAR(100)
AS
BEGIN
    SELECT a.AsignacionID, a.FechaAsignacion,
           r.ReparacionID, r.Estado, e.TipoEquipo, e.Modelo,
           u.Nombre AS NombreUsuario,
           t.Nombre AS NombreTecnico, t.Especialidad
    FROM Asignaciones a
    JOIN Reparaciones r ON a.ReparacionID = r.ReparacionID
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    JOIN Usuarios u ON e.UsuarioID = u.UsuarioID
    JOIN Tecnicos t ON a.TecnicoID = t.TecnicoID
    WHERE r.Estado LIKE '%' + @texto + '%'
       OR e.TipoEquipo LIKE '%' + @texto + '%'
       OR e.Modelo LIKE '%' + @texto + '%'
       OR u.Nombre LIKE '%' + @texto + '%'
       OR t.Nombre LIKE '%' + @texto + '%'
       OR t.Especialidad LIKE '%' + @texto + '%'
    ORDER BY a.FechaAsignacion DESC;
END
GO

IF OBJECT_ID('sp_Asignaciones_ObtenerPorId', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_ObtenerPorId;
GO

CREATE PROCEDURE sp_Asignaciones_ObtenerPorId
    @id INT
AS
BEGIN
    SELECT a.AsignacionID, a.ReparacionID, a.TecnicoID, a.FechaAsignacion
    FROM Asignaciones a
    WHERE a.AsignacionID = @id;
END
GO

IF OBJECT_ID('sp_Asignaciones_ListarReparaciones', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_ListarReparaciones;
GO

CREATE PROCEDURE sp_Asignaciones_ListarReparaciones
AS
BEGIN
    SELECT r.ReparacionID,
           'Reparacion #' + CONVERT(VARCHAR(10), r.ReparacionID) + ' - ' +
           e.TipoEquipo + ' ' + e.Modelo + ' (' + r.Estado + ')' AS NombreReparacion
    FROM Reparaciones r
    JOIN Equipos e ON r.EquipoID = e.EquipoID
    ORDER BY r.ReparacionID DESC;
END
GO

IF OBJECT_ID('sp_Asignaciones_ListarTecnicos', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_ListarTecnicos;
GO

CREATE PROCEDURE sp_Asignaciones_ListarTecnicos
AS
BEGIN
    SELECT TecnicoID,
           Nombre + ' - ' + Especialidad AS NombreTecnico
    FROM Tecnicos
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('sp_Asignaciones_Insertar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_Insertar;
GO

CREATE PROCEDURE sp_Asignaciones_Insertar
    @reparacionId INT,
    @tecnicoId INT
AS
BEGIN
    -- la fecha de asignacion se llena sola con la fecha actual (GETDATE)
    INSERT INTO Asignaciones (ReparacionID, TecnicoID)
    VALUES (@reparacionId, @tecnicoId);
END
GO

IF OBJECT_ID('sp_Asignaciones_Actualizar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_Actualizar;
GO

CREATE PROCEDURE sp_Asignaciones_Actualizar
    @id INT,
    @reparacionId INT,
    @tecnicoId INT
AS
BEGIN
    UPDATE Asignaciones
    SET ReparacionID = @reparacionId,
        TecnicoID = @tecnicoId
    WHERE AsignacionID = @id;
END
GO

IF OBJECT_ID('sp_Asignaciones_Eliminar', 'P') IS NOT NULL
    DROP PROCEDURE sp_Asignaciones_Eliminar;
GO

CREATE PROCEDURE sp_Asignaciones_Eliminar
    @id INT
AS
BEGIN
    DELETE FROM Asignaciones WHERE AsignacionID = @id;
END
GO

PRINT 'Procedimientos almacenados creados correctamente.';
