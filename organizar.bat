@echo off
echo Creando carpeta Useless y moviendo archivos innecesarios...

mkdir "C:\Users\User1\source\repos\TechSystem2\Useless" 2>nul

:: Archivos y carpetas que no son del proyecto real (que esta en TechSystem2\)
move "C:\Users\User1\source\repos\TechSystem2\App_Code" "C:\Users\User1\source\repos\TechSystem2\Useless\App_Code" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Backup Base de datos" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\LOGINCSHARP-master" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\SQL Script" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\TechSystemDiagram" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Default.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Default.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Usuarios.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Usuarios.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Equipos.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Equipos.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Tecnicos.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Tecnicos.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Login.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Login.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Home.aspx" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Home.aspx.cs" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Style.css" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1
move "C:\Users\User1\source\repos\TechSystem2\Web.config" "C:\Users\User1\source\repos\TechSystem2\Useless\" >nul 2>&1

echo Hecho!
pause
