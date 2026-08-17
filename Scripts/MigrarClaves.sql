-- ====================================================================
-- TechSystem - Migrar claves viejas al formato encriptado (hash)
--
-- Antes las claves se guardaban en texto plano. Ahora se guardan
-- encriptadas con SHA256. Este script convierte las claves que ya
-- existian para que los usuarios puedan seguir entrando SIN tener
-- que cambiar su clave.
--
-- COMO USARLO:
-- 1. Abra SQL Server Management Studio (SSMS)
-- 2. Ejecute este script UNA sola vez
--
-- ¡IMPORTANTE! No lo ejecute dos veces: si la clave ya esta
-- encriptada, se encriptaria otra vez y el login no funcionaria.
-- ====================================================================

USE TechSystemDB;
GO

-- 1) agrandamos la columna Clave: el hash ocupa 64 caracteres
--    y antes la columna era mas corta (50), por eso cortaba el hash
ALTER TABLE Usuarios ALTER COLUMN Clave VARCHAR(64);
GO

-- 2) convertimos las claves viejas (texto plano) al formato encriptado
UPDATE Usuarios
SET Clave = LOWER(CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CONVERT(VARCHAR(100), Clave)), 2));
GO

PRINT 'Claves migradas correctamente.';
