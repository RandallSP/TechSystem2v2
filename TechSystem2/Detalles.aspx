<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detalles.aspx.cs" Inherits="TechSystem2.Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Detalles de Reparacion - TechSystem</title>
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
                <h2>Buscar Detalles</h2>
                <div class="campo-busqueda">
                    <asp:TextBox ID="txtBuscar" runat="server" placeholder="Buscar por descripcion, estado, equipo o usuario..."></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn-buscar" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn-limpiar" OnClick="btnLimpiar_Click" />
                </div>
            </div>

            <div class="tarjeta">
                <h2>Datos del Detalle</h2>

                <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>

                <div class="form-columnas">
                    <div>
                        <label>Reparacion:</label>
                        <asp:DropDownList ID="ddlReparacion" runat="server"></asp:DropDownList>
                    </div>
                    <div class="full-width">
                        <label>Descripcion:</label>
                        <asp:TextBox ID="txtDescripcion" runat="server" TextMode="Multiline" Rows="3"
                            placeholder="Describa el trabajo realizado"></asp:TextBox>
                    </div>
                    <div>
                        <label>Fecha Inicio:</label>
                        <asp:TextBox ID="txtFechaInicio" runat="server" placeholder="ej: 2026-07-16 10:15 (opcional)"></asp:TextBox>
                    </div>
                    <div>
                        <label>Fecha Fin:</label>
                        <asp:TextBox ID="txtFechaFin" runat="server" placeholder="ej: 2026-07-16 12:00 (opcional)"></asp:TextBox>
                    </div>
                </div>

                <div class="botones">
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn-guardar" OnClick="btnGuardar_Click" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn-limpiar" OnClick="btnNuevo_Click" />
                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                </div>

                <asp:Panel ID="pnlConfirmar" runat="server" CssClass="panel-confirmacion" Visible="false">
                    <p>Seguro que desea eliminar este detalle? No se puede deshacer.</p>
                    <div class="botones" style="justify-content: center;">
                        <asp:Button ID="btnSi" runat="server" Text="Si, Eliminar" CssClass="btn-eliminar" OnClick="btnSi_Click" />
                        <asp:Button ID="btnNo" runat="server" Text="No, Cancelar" CssClass="btn-limpiar" OnClick="btnNo_Click" />
                    </div>
                </asp:Panel>
            </div>

            <div class="tarjeta">
                <h2>Lista de Detalles</h2>
                <div class="grid-container">
                    <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False"
                        CssClass="gridview" DataKeyNames="DetalleID"
                        OnRowCommand="gvDetalles_RowCommand" OnRowDataBound="gvDetalles_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="DetalleID" HeaderText="ID" />
                            <asp:BoundField DataField="ReparacionID" HeaderText="Rep #" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <div class="acciones">
                                        <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandName="Seleccionar" 
                                            CommandArgument='<%# Eval("DetalleID") %>' Text="Seleccionar" />
                                        <asp:LinkButton ID="lnkEliminar" runat="server" CommandName="Eliminar" 
                                            CommandArgument='<%# Eval("DetalleID") %>' Text="Eliminar" />
                                    </div>
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
