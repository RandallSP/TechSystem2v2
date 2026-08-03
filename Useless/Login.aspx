<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Iniciar Sesion - TechSystem</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="header">
            <div class="titulo-app">TechSystem - Soporte Tecnico</div>
        </div>

        <div class="contenedor">
            <div style="max-width: 450px; margin: 80px auto;">

                <div class="tarjeta" style="text-align: center;">
                    <h2 style="border-bottom: none; margin-bottom: 5px;">Iniciar Sesion</h2>
                    <p style="color: #8b949e; font-size: 14px; margin-bottom: 25px;">
                        Ingrese sus credenciales para acceder al sistema
                    </p>

                    <asp:Label ID="lblMensaje" runat="server" CssClass="lbl-mensaje"></asp:Label>

                    <div style="text-align: left;">
                        <label>Correo Electronico:</label>
                        <asp:TextBox ID="txtCorreo" runat="server" placeholder="Ej: usuario@correo.com"></asp:TextBox>

                        <label>Clave:</label>
                        <asp:TextBox ID="txtClave" runat="server" TextMode="Password" placeholder="Escriba su clave"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn-guardar" OnClick="btnIngresar_Click" style="width: 100%; margin-top: 10px;" />

                </div>

            </div>
        </div>

    </form>
</body>
</html>
