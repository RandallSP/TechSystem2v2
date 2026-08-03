<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tecnicos.aspx.cs" Inherits="Tecnicos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Tecnicos - TechSystem</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
    <form id="form1" runat="server">

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

            <div class="tarjeta">
                <h2>Buscar Tecnicos</h2>
                <div class="campo-busqueda">
                    <asp:TextBox ID="txtBuscarTecnico" runat="server" placeholder="Buscar por nombre o especialidad..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiarBusqueda" runat="server" Text="Limpiar" CssClass="btn-limpiar" OnClick="btnLimpiarBusqueda_Click" />
                </div>
            </div>

            <div class="tarjeta">
                <h2>Datos del Tecnico</h2>
                <asp:HiddenField ID="hfTecnicoID" runat="server" />

                <div class="form-columnas">
                    <div>
                        <label>Nombre:</label>
                        <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre completo del tecnico"></asp:TextBox>
                    </div>
                    <div>
                        <label>Especialidad:</label>
                        <asp:TextBox ID="txtEspecialidad" runat="server" placeholder="Ej: Hardware, Software, Redes..."></asp:TextBox>
                    </div>
                </div>

                <div class="botones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-limpiar" OnClick="btnNuevo_Click" />
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </div>

                <asp:Panel ID="pnlConfirmacion" runat="server" CssClass="panel-confirmacion" Visible="false">
                    <p>¿Esta seguro de que desea eliminar este tecnico? Esta accion no se puede deshacer.</p>
                    <div class="botones" style="justify-content: center;">
                        <asp:Button ID="btnSiEliminar" runat="server" Text="Si, Eliminar" CssClass="btn-eliminar" OnClick="btnSiEliminar_Click" />
                        <asp:Button ID="btnNoCancelar" runat="server" Text="No, Cancelar" CssClass="btn-limpiar" OnClick="btnNoCancelar_Click" />
                    </div>
                </asp:Panel>
            </div>

            <div class="tarjeta">
                <h2>Lista de Tecnicos</h2>
                <div class="grid-container">
                    <asp:GridView ID="gvTecnicos" runat="server" AutoGenerateColumns="False"
                        CssClass="gridview" DataKeyNames="TecnicoID"
                        OnRowCommand="gvTecnicos_RowCommand" OnRowDataBound="gvTecnicos_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="TecnicoID" HeaderText="ID" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandName="Seleccionar" 
                                        CommandArgument='<%# Eval("TecnicoID") %>' Text="Seleccionar" />
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("TecnicoID") %>' Text="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>

        <div class="footer">
            TechSystem - Hecho en C# - Curso de Programacion II &copy; 2026
        </div>
    </form>
</body>
</html>
