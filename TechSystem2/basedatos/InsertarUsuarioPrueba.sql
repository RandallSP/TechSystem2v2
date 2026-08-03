-- Insertar usuario de prueba para login
USE TechSystemDB;
GO

INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono, Clave)
VALUES ('Administrador', 'admin@sistema.com', '8888-8888', 'admin123');
GO

PRINT 'Usuario de prueba creado: admin@sistema.com / admin123';
GO
