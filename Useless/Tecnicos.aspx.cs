using System; // por si acaso
using System.Data;

/// <summary>
/// Pagina de mantenimiento de Tecnicos (CRUD completo).
/// </summary>
public partial class Tecnicos : System.Web.UI.Page
{
    private TecnicoDatos tecnicoDatos = new TecnicoDatos();

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
            LlenarGrid();
        }
    }

    private void LlenarGrid()
    {
        try
        {
            DataTable tabla = tecnicoDatos.ListarTodos();
            gvTecnicos.DataSource = tabla;
            gvTecnicos.DataBind();
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
            string texto = txtBuscarTecnico.Text.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                LlenarGrid();
                lblMensaje.Text = "";
            }
            else
            {
                DataTable tabla = tecnicoDatos.Buscar(texto);
                gvTecnicos.DataSource = tabla;
                gvTecnicos.DataBind();
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
        txtBuscarTecnico.Text = "";
        LlenarGrid();
        lblMensaje.Text = "";
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba el nombre del tecnico.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }
            if (string.IsNullOrEmpty(txtEspecialidad.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba la especialidad del tecnico.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string especialidad = txtEspecialidad.Text.Trim();

            if (string.IsNullOrEmpty(hfTecnicoID.Value))
            {
                tecnicoDatos.Insertar(nombre, especialidad);
                lblMensaje.Text = "Tecnico guardado exitosamente.";
            }
            else
            {
                int tecnicoID = Convert.ToInt32(hfTecnicoID.Value);
                tecnicoDatos.Actualizar(tecnicoID, nombre, especialidad);
                lblMensaje.Text = "Tecnico actualizado exitosamente.";
            }

            lblMensaje.CssClass = "mensaje-exito";
            LimpiarFormulario();
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al guardar el tecnico: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    protected void gvTecnicos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            int tecnicoID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Seleccionar")
            {
                DataTable tabla = tecnicoDatos.ObtenerPorId(tecnicoID);
                if (tabla.Rows.Count > 0)
                {
                    DataRow fila = tabla.Rows[0];
                    hfTecnicoID.Value = fila["TecnicoID"].ToString();
                    txtNombre.Text = fila["Nombre"].ToString();
                    txtEspecialidad.Text = fila["Especialidad"].ToString();
                    lblMensaje.Text = "Tecnico cargado. Modifique los datos y presione Guardar.";
                    lblMensaje.CssClass = "mensaje-exito";
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                hfTecnicoID.Value = tecnicoID.ToString();
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

    protected void gvTecnicos_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            int tecnicoID = Convert.ToInt32(hfTecnicoID.Value);
            tecnicoDatos.Eliminar(tecnicoID);
            lblMensaje.Text = "Tecnico eliminado exitosamente.";
            lblMensaje.CssClass = "mensaje-exito";
            pnlConfirmacion.Visible = false;
            LimpiarFormulario();
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al eliminar el tecnico: " + ex.Message;
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
        hfTecnicoID.Value = "";
        txtNombre.Text = "";
        txtEspecialidad.Text = "";
    }
}
