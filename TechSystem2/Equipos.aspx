<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Equipos.aspx.cs" Inherits="TechSystem2.Equipos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Equipos - TechSystem</title>
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
                <a href="Reparaciones.aspx">Reparaciones</a>
                <a href="Detalles.aspx">Detalles</a>
                <a href="Asignaciones.aspx">Asignaciones</a>
                <asp:LinkButton ID="btnSalir" runat="server" Text="Salir" OnClick="btnSalir_Click" style="color: #f85149;" />
            </div>
        </div>

        <div class="contenedor">

            <div class="tarjeta">
                <h2>Buscar Equipos</h2>
                <div class="campo-busqueda">
                    <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por tipo o modelo..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn-limpiar" OnClick="btnLimpiar_Click" />
                </div>
            </div>

            <div class="tarjeta">
                <h2>Datos del Equipo</h2>

                <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>

                <div class="form-columnas">
                    <div>
                        <label>Tipo:</label>
                        <asp:TextBox ID="txtTipo" runat="server" placeholder="Laptop, Desktop, Impresora..."></asp:TextBox>
                    </div>
                    <div>
                        <label>Modelo:</label>
                        <asp:TextBox ID="txtModelo" runat="server" placeholder="Modelo del equipo"></asp:TextBox>
                    </div>
                    <div>
                        <label>Usuario:</label>
                        <asp:DropDownList ID="ddlUsuario" runat="server"></asp:DropDownList>
                    </div>
                </div>

                <div class="botones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-limpiar" OnClick="btnNuevo_Click" />
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </div>

                <asp:Panel ID="pnlConfirmar" runat="server" CssClass="panel-confirmacion" Visible="false">
                    <p>Seguro que desea eliminar este equipo? No se puede deshacer.</p>
                    <div class="botones" style="justify-content: center;">
                        <asp:Button ID="btnSi" runat="server" Text="Si, Eliminar" CssClass="btn-eliminar" OnClick="btnSi_Click" />
                        <asp:Button ID="btnNo" runat="server" Text="No, Cancelar" CssClass="btn-limpiar" OnClick="btnNo_Click" />
                    </div>
                </asp:Panel>
            </div>

            <div class="tarjeta">
                <h2>Lista de Equipos</h2>
                <div class="grid-container">
                    <asp:GridView ID="gvEquipos" runat="server" AutoGenerateColumns="False"
                        CssClass="gridview" DataKeyNames="EquipoID"
                        OnRowCommand="gvEquipos_RowCommand" OnRowDataBound="gvEquipos_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="EquipoID" HeaderText="ID" />
                            <asp:BoundField DataField="TipoEquipo" HeaderText="Tipo" />
                            <asp:BoundField DataField="Modelo" HeaderText="Modelo" />
                            <asp:BoundField DataField="NombreUsuario" HeaderText="Usuario" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandName="Seleccionar" 
                                        CommandArgument='<%# Eval("EquipoID") %>' Text="Seleccionar" />
                                    <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("EquipoID") %>' Text="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
