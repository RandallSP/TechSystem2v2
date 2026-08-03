-- =============================================
-- Script para crear la Base de Datos TechSystemDB
-- Sistema de Soporte Tecnico
-- Fecha: 2026
-- =============================================

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
-- =============================================
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    CorreoElectronico NVARCHAR(150) NOT NULL,
    Telefono NVARCHAR(20) NULL
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

-- Usuarios
INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono) VALUES
('Randall Sanchez', 'randall@uhispano.ac.cr', '8888-1111'),
('Maria Rodriguez', 'maria@uhispano.ac.cr', '8888-2222'),
('Carlos Jimenez', 'carlos@uhispano.ac.cr', '8888-3333');
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
