using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar las asignaciones del sistema
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (AsignacionNegocio)
public partial class Asignaciones : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected DropDownList ddlReparacion;
    protected DropDownList ddlTecnico;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvAsignaciones;

    // capa de negocio
    AsignacionNegocio negocio = new AsignacionNegocio();

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
            CargarReparaciones();
            CargarTecnicos();
            CargarTabla();
        }
    }

    // llena el combo con las reparaciones
    public void CargarReparaciones()
    {
        DataTable dt = negocio.ListarReparacionesParaCombo();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar las reparaciones.";
            return;
        }
        ddlReparacion.DataSource = dt;
        ddlReparacion.DataTextField = "NombreReparacion";
        ddlReparacion.DataValueField = "ReparacionID";
        ddlReparacion.DataBind();
        ddlReparacion.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }

    // llena el combo con los tecnicos
    public void CargarTecnicos()
    {
        DataTable dt = negocio.ListarTecnicosParaCombo();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los tecnicos.";
            return;
        }
        ddlTecnico.DataSource = dt;
        ddlTecnico.DataTextField = "NombreTecnico";
        ddlTecnico.DataValueField = "TecnicoID";
        ddlTecnico.DataBind();
        ddlTecnico.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }

    // Carga todas las asignaciones en el grid
    public void CargarTabla()
    {
        DataTable dt = negocio.ListarAsignaciones();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los datos.";
            return;
        }
        gvAsignaciones.DataSource = dt;
        gvAsignaciones.DataBind();
    }

    // Buscar asignaciones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = negocio.BuscarAsignaciones(texto);
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al buscar.";
            return;
        }
        gvAsignaciones.DataSource = dt;
        gvAsignaciones.DataBind();
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
        string mensaje = negocio.GuardarAsignacion(id, ddlReparacion.SelectedValue, ddlTecnico.SelectedValue);

        if (mensaje == "")
        {
            LimpiarFormulario();
            CargarTabla();
            if (id == "")
            {
                lblMensaje.Text = "Asignacion guardada!";
            }
            else
            {
                lblMensaje.Text = "Asignacion actualizada!";
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
    protected void gvAsignaciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = negocio.ObtenerAsignacion(id);
            if (dt == null)
            {
                lblMensaje.Text = "Ocurrio un error al cargar la asignacion.";
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["AsignacionID"].ToString();
                ddlReparacion.SelectedValue = f["ReparacionID"].ToString();
                ddlTecnico.SelectedValue = f["TecnicoID"].ToString();
                lblMensaje.Text = "Asignacion cargada. Modifique y presione Guardar.";
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
    protected void gvAsignaciones_RowDataBound(object sender, GridViewRowEventArgs e)
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
        string mensaje = negocio.EliminarAsignacion(id);

        if (mensaje == "")
        {
            lblMensaje.Text = "Asignacion eliminada!";
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
        if (ddlReparacion.Items.Count > 0) ddlReparacion.SelectedIndex = 0;
        if (ddlTecnico.Items.Count > 0) ddlTecnico.SelectedIndex = 0;
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
