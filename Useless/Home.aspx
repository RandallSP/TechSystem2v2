<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TechSystem2.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Inicio - TechSystem</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- Encabezado con navegacion -->
        <div class="header">
            <div class="titulo-app">TechSystem - Soporte Tecnico</div>
            <div class="links-nav">
                <asp:Label ID="lblUsuarioNav" runat="server" style="color: #58a6ff; margin-right: 15px; font-weight: bold;"></asp:Label>
                <a href="Home.aspx">Inicio</a>
                <a href="Usuarios.aspx">Usuarios</a>
                <a href="Equipos.aspx">Equipos</a>
                <a href="Tecnicos.aspx">Tecnicos</a>
                <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesion" CssClass="btn-limpiar" OnClick="btnCerrarSesion_Click" style="margin-left: 10px;" />
            </div>
        </div>

        <div class="contenedor">

            <!-- Bienvenida -->
            <div class="tarjeta" style="text-align: center; padding: 60px 30px;">
                <h2 style="font-size: 28px; border-bottom: none; margin-bottom: 10px;">
                    <asp:Label ID="lblBienvenida" runat="server"></asp:Label>
                </h2>
                <p style="color: #8b949e; font-size: 16px; margin-bottom: 20px;">
                    Has iniciado sesion correctamente en el sistema de soporte tecnico
                </p>
                <hr style="border: 1px solid #30363d; margin: 25px 0;" />
                <p style="color: #c9d1d9; font-size: 15px;">
                    Use el menu de arriba para navegar entre las diferentes secciones del sistema.
                </p>
            </div>

            <!-- Resumen del sistema -->
            <div class="tarjeta">
                <h2>Que puedes hacer en este sistema</h2>
                <ul style="line-height: 2; font-size: 15px;">
                    <li><strong>Usuarios:</strong> Agregar, modificar, buscar y eliminar usuarios del sistema</li>
                    <li><strong>Equipos:</strong> Registrar equipos y asignarlos a un usuario</li>
                    <li><strong>Tecnicos:</strong> Administrar los tecnicos de soporte y sus especialidades</li>
                </ul>
            </div>

        </div>

    </form>
</body>
</html>
