using System; // por si acaso
using System.Data;

/// <summary>
/// Pagina de mantenimiento de Equipos (CRUD completo).
/// </summary>
public partial class Equipos : System.Web.UI.Page
{
    private EquipoDatos equipoDatos = new EquipoDatos();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Verificamos que el usuario haya iniciado sesion
        if (Session["NombreUsuario"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            CargarDropdownUsuarios();
            LlenarGrid();
        }
    }

    private void CargarDropdownUsuarios()
    {
        try
        {
            DataTable tabla = equipoDatos.ListarUsuarios();
            ddlUsuario.DataSource = tabla;
            ddlUsuario.DataTextField = "Nombre";
            ddlUsuario.DataValueField = "UsuarioID";
            ddlUsuario.DataBind();
            ddlUsuario.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Seleccione un usuario --", ""));
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al cargar usuarios: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    private void LlenarGrid()
    {
        try
        {
            DataTable tabla = equipoDatos.ListarTodos();
            gvEquipos.DataSource = tabla;
            gvEquipos.DataBind();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            string texto = txtBuscarEquipo.Text.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                LlenarGrid();
                lblMensaje.Text = "";
            }
            else
            {
                DataTable tabla = equipoDatos.Buscar(texto);
                gvEquipos.DataSource = tabla;
                gvEquipos.DataBind();
                lblMensaje.Text = "Resultados encontrados: " + tabla.Rows.Count;
                lblMensaje.CssClass = "mensaje-exito";
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al buscar: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
    {
        txtBuscarEquipo.Text = "";
        LlenarGrid();
        lblMensaje.Text = "";
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtTipoEquipo.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba el tipo de equipo.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }
            if (string.IsNullOrEmpty(txtModelo.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba el modelo del equipo.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }
            if (string.IsNullOrEmpty(ddlUsuario.SelectedValue))
            {
                lblMensaje.Text = "Por favor seleccione un usuario para este equipo.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }

            string tipoEquipo = txtTipoEquipo.Text.Trim();
            string modelo = txtModelo.Text.Trim();
            int usuarioID = Convert.ToInt32(ddlUsuario.SelectedValue);

            if (string.IsNullOrEmpty(hfEquipoID.Value))
            {
                equipoDatos.Insertar(tipoEquipo, modelo, usuarioID);
                lblMensaje.Text = "Equipo guardado exitosamente.";
            }
            else
            {
                int equipoID = Convert.ToInt32(hfEquipoID.Value);
                equipoDatos.Actualizar(equipoID, tipoEquipo, modelo, usuarioID);
                lblMensaje.Text = "Equipo actualizado exitosamente.";
            }

            lblMensaje.CssClass = "mensaje-exito";
            LimpiarFormulario();
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al guardar el equipo: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    protected void gvEquipos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            int equipoID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Seleccionar")
            {
                DataTable tabla = equipoDatos.ObtenerPorId(equipoID);
                if (tabla.Rows.Count > 0)
                {
                    DataRow fila = tabla.Rows[0];
                    hfEquipoID.Value = fila["EquipoID"].ToString();
                    txtTipoEquipo.Text = fila["TipoEquipo"].ToString();
                    txtModelo.Text = fila["Modelo"].ToString();
                    ddlUsuario.SelectedValue = fila["UsuarioID"].ToString();
                    lblMensaje.Text = "Equipo cargado. Modifique los datos y presione Guardar.";
                    lblMensaje.CssClass = "mensaje-exito";
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                hfEquipoID.Value = equipoID.ToString();
                pnlConfirmacion.Visible = true;
                lblMensaje.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    protected void gvEquipos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkSeleccionar = (LinkButton)e.Row.FindControl("lnkSeleccionar");
            LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminar");
            if (lnkSeleccionar != null) lnkSeleccionar.CssClass = "btn-accion";
            if (lnkEliminar != null) lnkEliminar.CssClass = "btn-eliminar";
        }
    }

    protected void btnSiEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            int equipoID = Convert.ToInt32(hfEquipoID.Value);
            equipoDatos.Eliminar(equipoID);
            lblMensaje.Text = "Equipo eliminado exitosamente.";
            lblMensaje.CssClass = "mensaje-exito";
            pnlConfirmacion.Visible = false;
            LimpiarFormulario();
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al eliminar el equipo: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
            pnlConfirmacion.Visible = false;
        }
    }

    protected void btnNoCancelar_Click(object sender, EventArgs e)
    {
        pnlConfirmacion.Visible = false;
        lblMensaje.Text = "";
    }

    private void LimpiarFormulario()
    {
        hfEquipoID.Value = "";
        txtTipoEquipo.Text = "";
        txtModelo.Text = "";
        if (ddlUsuario.Items.Count > 0) ddlUsuario.SelectedIndex = 0;
    }
}
