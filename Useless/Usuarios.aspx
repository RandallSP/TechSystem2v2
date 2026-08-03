<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Usuarios.aspx.cs" Inherits="Usuarios" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Usuarios - TechSystem</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- Encabezado con navegacion -->
        <div class="header">
            <div class="titulo-app">TechSystem - Soporte Tecnico</div>
            <div class="links-nav">
                <a href="Default.aspx">Inicio</a>
                <a href="Usuarios.aspx">Usuarios</a>
                <a href="Equipos.aspx">Equipos</a>
                <a href="Tecnicos.aspx">Tecnicos</a>
                <a href="Login.aspx" style="color: #f85149;">Cerrar Sesion</a>
            </div>
        </div>

        <div class="contenedor">

            <!-- ===== SECCION DE BUSQUEDA (FILTRO) ===== -->
            <div class="tarjeta">
                <h2>Buscar Usuarios</h2>
                <div class="campo-busqueda">
                    <asp:TextBox ID="txtBuscarUsuario" runat="server" placeholder="Buscar por nombre o correo..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiarBusqueda" runat="server" Text="Limpiar" CssClass="btn-limpiar" OnClick="btnLimpiarBusqueda_Click" />
                </div>
            </div>

            <!-- ===== FORMULARIO DE USUARIO ===== -->
            <div class="tarjeta">
                <h2>Datos del Usuario</h2>

                <!-- ID oculto (para saber si es agregar o modificar) -->
                <asp:HiddenField ID="hfUsuarioID" runat="server" />

                <div class="form-columnas">
                    <div>
                        <label>Nombre:</label>
                        <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre completo"></asp:TextBox>
                    </div>
                    <div>
                        <label>Correo Electronico:</label>
                        <asp:TextBox ID="txtCorreo" runat="server" placeholder="correo@ejemplo.com"></asp:TextBox>
                    </div>
                    <div>
                        <label>Telefono:</label>
                        <asp:TextBox ID="txtTelefono" runat="server" placeholder="8888-0000"></asp:TextBox>
                    </div>
                </div>

                <!-- Botones de accion -->
                <div class="botones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-limpiar" OnClick="btnNuevo_Click" />
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </div>

                <!-- Panel de confirmacion para eliminar (sin JavaScript) -->
                <asp:Panel ID="pnlConfirmacion" runat="server" CssClass="panel-confirmacion" Visible="false">
                    <p>¿Esta seguro de que desea eliminar este usuario? Esta accion no se puede deshacer.</p>
                    <div class="botones" style="justify-content: center;">
                        <asp:Button ID="btnSiEliminar" runat="server" Text="Si, Eliminar" CssClass="btn-eliminar" OnClick="btnSiEliminar_Click" />
                        <asp:Button ID="btnNoCancelar" runat="server" Text="No, Cancelar" CssClass="btn-limpiar" OnClick="btnNoCancelar_Click" />
                    </div>
                </asp:Panel>
            </div>

            <!-- ===== TABLA DE USUARIOS ===== -->
            <div class="tarjeta">
                <h2>Lista de Usuarios</h2>
                <div class="grid-container">
                    <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False"
                        CssClass="gridview" DataKeyNames="UsuarioID"
                        OnRowCommand="gvUsuarios_RowCommand"
                        OnRowDataBound="gvUsuarios_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="UsuarioID" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electronico" />
                            <asp:BoundField DataField="Telefono" HeaderText="Telefono" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandName="Seleccionar" 
                                        CommandArgument='<%# Eval("UsuarioID") %>' Text="Seleccionar" />
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("UsuarioID") %>' Text="Eliminar"
                                        OnClientClick="return false;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>

        <!-- Pie de pagina -->
        <div class="footer">
            TechSystem - Hecho en C# - Curso de Programacion II &copy; 2026
        </div>
    </form>
</body>
</html>
