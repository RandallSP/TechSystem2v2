using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar las reparaciones del sistema
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (ReparacionNegocio)
public partial class Reparaciones : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected DropDownList ddlEquipo;
    protected DropDownList ddlEstado;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvReparaciones;

    // capa de negocio
    ReparacionNegocio negocio = new ReparacionNegocio();

    protected void Page_Load(object sender, EventArgs e)
    {
        // si no ha iniciado sesion lo mandamos al login
        if (Session["nombre"] == null)
        {
            Response.Redirect("Default.aspx");
            return;
        }

        if (!IsPostBack)
        {
            CargarEquipos();
            CargarTabla();
        }
    }

    // llena el combo con los equipos
    public void CargarEquipos()
    {
        DataTable dt = negocio.ListarEquiposParaCombo();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los equipos.";
            return;
        }
        ddlEquipo.DataSource = dt;
        ddlEquipo.DataTextField = "NombreEquipo";
        ddlEquipo.DataValueField = "EquipoID";
        ddlEquipo.DataBind();
        ddlEquipo.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }

    // Carga todas las reparaciones en el grid
    public void CargarTabla()
    {
        DataTable dt = negocio.ListarReparaciones();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los datos.";
            return;
        }
        gvReparaciones.DataSource = dt;
        gvReparaciones.DataBind();
    }

    // Buscar reparaciones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = negocio.BuscarReparaciones(texto);
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al buscar.";
            return;
        }
        gvReparaciones.DataSource = dt;
        gvReparaciones.DataBind();
        lblMensaje.Text = "Se encontraron " + dt.Rows.Count + " resultados";
    }

    // Limpiar busqueda
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtBuscar.Text = "";
        CargarTabla();
        lblMensaje.Text = "";
    }

    // Guardar o actualizar (las validaciones estan en la capa de negocio)
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        string id = txtID.Text;
        string mensaje = negocio.GuardarReparacion(id, ddlEquipo.SelectedValue, ddlEstado.SelectedValue);

        if (mensaje == "")
        {
            LimpiarFormulario();
            CargarTabla();
            if (id == "")
            {
                lblMensaje.Text = "Reparacion guardada!";
            }
            else
            {
                lblMensaje.Text = "Reparacion actualizada!";
            }
        }
        else
        {
            lblMensaje.Text = mensaje;
        }
    }

    // Boton nuevo
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    // Eventos del grid (Seleccionar y Eliminar)
    protected void gvReparaciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = negocio.ObtenerReparacion(id);
            if (dt == null)
            {
                lblMensaje.Text = "Ocurrio un error al cargar la reparacion.";
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["ReparacionID"].ToString();
                ddlEquipo.SelectedValue = f["EquipoID"].ToString();
                ddlEstado.SelectedValue = f["Estado"].ToString();
                lblMensaje.Text = "Reparacion cargada. Modifique y presione Guardar.";
            }
        }
        else if (e.CommandName == "Eliminar")
        {
            txtID.Text = id.ToString();
            pnlConfirmar.Visible = true;
            lblMensaje.Text = "";
        }
    }

    // Estilos de botones del grid
    protected void gvReparaciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton sel = (LinkButton)e.Row.FindControl("lnkSeleccionar");
            LinkButton del = (LinkButton)e.Row.FindControl("lnkEliminar");
            if (sel != null) sel.CssClass = "btn-accion";
            if (del != null) del.CssClass = "btn-eliminar";
        }
    }

    // Confirmar eliminar
    protected void btnSi_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txtID.Text);
        string mensaje = negocio.EliminarReparacion(id);

        if (mensaje == "")
        {
            lblMensaje.Text = "Reparacion eliminada!";
            LimpiarFormulario();
            CargarTabla();
        }
        else
        {
            lblMensaje.Text = mensaje;
        }
        pnlConfirmar.Visible = false;
    }

    // Cancelar eliminar
    protected void btnNo_Click(object sender, EventArgs e)
    {
        pnlConfirmar.Visible = false;
        lblMensaje.Text = "";
    }

    // Deja los campos en blanco
    public void LimpiarFormulario()
    {
        txtID.Text = "";
        if (ddlEquipo.Items.Count > 0) ddlEquipo.SelectedIndex = 0;
        if (ddlEstado.Items.Count > 0) ddlEstado.SelectedIndex = 0;
    }

    // Boton Salir: borra la sesion y vuelve al login
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["id"] = null;
        Session["nombre"] = null;
        Response.Redirect("Default.aspx");
    }
}

}
