<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TechSystem2.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>TechSystem - Departamento Tecnico</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- PANEL DE LOGIN: se muestra cuando no ha iniciado sesion -->
        <asp:Panel ID="pnlLogin" runat="server">
            <div class="header">
                <div class="titulo-app">TechSystem - Soporte Tecnico</div>
            </div>
            <div class="contenedor">
                <div style="max-width: 450px; margin: 80px auto;">
                    <div class="tarjeta" style="text-align: center;">
                        <h2 style="border-bottom: none; margin-bottom: 5px;">Iniciar Sesion</h2>
                        <p style="color: #8b949e; font-size: 14px; margin-bottom: 25px;">
                            Ingrese sus credenciales para acceder
                        </p>
                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
                        <div style="text-align: left;">
                            <label>Correo:</label>
                            <asp:TextBox ID="txtCorreo" runat="server" placeholder="Ej: admin@sistema.com"></asp:TextBox>
                            <label>Clave:</label>
                            <asp:TextBox ID="txtClave" runat="server" TextMode="Password" placeholder="Escriba su clave"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="btn-guardar" OnClick="btnEntrar_Click" style="width: 100%; margin-top: 10px;" />
                    </div>
                </div>
            </div>
        </asp:Panel>

        <!-- PANEL PRINCIPAL: se muestra cuando ya inicio sesion -->
        <asp:Panel ID="pnlSistema" runat="server">
            <div class="header">
                <div class="titulo-app">TechSystem - Soporte Tecnico</div>
                <div class="links-nav">
                    <span style="color: #58a6ff; margin-right: 10px;">
                        <asp:Label ID="lblNombre" runat="server"></asp:Label>
                    </span>
                    <a href="Default.aspx">Inicio</a>
                    <a href="Usuarios.aspx">Usuarios</a>
                    <a href="Equipos.aspx">Equipos</a>
                    <a href="Tecnicos.aspx">Tecnicos</a>
                    <a href="Reparaciones.aspx">Reparaciones</a>
                    <a href="Detalles.aspx">Detalles</a>
                    <a href="Asignaciones.aspx">Asignaciones</a>
                    <asp:Button ID="btnSalir" runat="server" Text="Salir" CssClass="btn-limpiar" OnClick="btnSalir_Click" style="margin-left: 10px;" />
                </div>
            </div>
            <div class="contenedor">
                <div class="tarjeta" style="text-align: center; padding: 60px 30px;">
                    <h2 style="font-size: 28px; border-bottom: none; margin-bottom: 10px;">
                        Sistema de Soporte Tecnico
                    </h2>
                    <p style="color: #8b949e; font-size: 16px; margin-bottom: 30px;">
                        Gestion de usuarios, equipos y tecnicos
                    </p>
                    <hr style="border: 1px solid #30363d; margin: 25px 0;" />
                    <p style="color: #58a6ff; font-size: 15px;">Dr. Randall Sanchez Perez</p>
                    <p style="color: #8b949e; font-size: 14px;">Universidad Hispanoamericana, 2026</p>
                </div>
                <div class="tarjeta">
                    <h2>Que puedes hacer</h2>
                    <ul style="line-height: 2; font-size: 15px;">
                        <li><strong>Usuarios:</strong> Agregar, modificar, buscar y eliminar</li>
                        <li><strong>Equipos:</strong> Registrar equipos y asignarlos a usuarios</li>
                        <li><strong>Tecnicos:</strong> Administrar tecnicos y especialidades</li>
                    </ul>
                </div>
            </div>
        </asp:Panel>

    </form>
</body>
</html>
