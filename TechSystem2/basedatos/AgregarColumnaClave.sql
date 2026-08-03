-- =============================================
-- Script para Login en TechSystemDB
-- Agrega la columna Clave a la tabla Usuarios
-- Ejecutar en: DESKTOP-P6SNJH4\SQLEXPRESS
-- Base de datos: TechSystemDB
-- =============================================

USE TechSystemDB;
GO

-- Agregar la columna Clave a la tabla Usuarios si no existe
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Usuarios' AND COLUMN_NAME = 'Clave'
)
BEGIN
    ALTER TABLE Usuarios ADD Clave NVARCHAR(50) NULL;
    PRINT 'Columna Clave agregada correctamente.';
END
ELSE
BEGIN
    PRINT 'La columna Clave ya existe en la tabla Usuarios.';
END
GO

-- Actualizar usuarios existentes con una clave por defecto
-- (para que puedan iniciar sesion con los datos actuales)
UPDATE Usuarios SET Clave = '123' WHERE Clave IS NULL;
GO

PRINT 'Script completado. Todos los usuarios ahora tienen clave.';
GO
