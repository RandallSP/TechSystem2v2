using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar los detalles de reparacion
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (DetalleNegocio)
public partial class Detalles : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected DropDownList ddlReparacion;
    protected TextBox txtDescripcion;
    protected TextBox txtFechaInicio;
    protected TextBox txtFechaFin;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvDetalles;

    // capa de negocio
    DetalleNegocio negocio = new DetalleNegocio();

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

    // Carga todos los detalles en el grid
    public void CargarTabla()
    {
        DataTable dt = negocio.ListarDetalles();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los datos.";
            return;
        }
        gvDetalles.DataSource = dt;
        gvDetalles.DataBind();
    }

    // Buscar detalles
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = negocio.BuscarDetalles(texto);
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al buscar.";
            return;
        }
        gvDetalles.DataSource = dt;
        gvDetalles.DataBind();
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
        string mensaje = negocio.GuardarDetalle(id, ddlReparacion.SelectedValue, txtDescripcion.Text, txtFechaInicio.Text, txtFechaFin.Text);

        if (mensaje == "")
        {
            LimpiarFormulario();
            CargarTabla();
            if (id == "")
            {
                lblMensaje.Text = "Detalle guardado!";
            }
            else
            {
                lblMensaje.Text = "Detalle actualizado!";
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
    protected void gvDetalles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = negocio.ObtenerDetalle(id);
            if (dt == null)
            {
                lblMensaje.Text = "Ocurrio un error al cargar el detalle.";
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["DetalleID"].ToString();
                ddlReparacion.SelectedValue = f["ReparacionID"].ToString();
                txtDescripcion.Text = f["Descripcion"].ToString();

                // las fechas pueden estar vacias en la base de datos
                if (f["FechaInicio"] == DBNull.Value)
                {
                    txtFechaInicio.Text = "";
                }
                else
                {
                    txtFechaInicio.Text = Convert.ToDateTime(f["FechaInicio"]).ToString("yyyy-MM-dd HH:mm");
                }

                if (f["FechaFin"] == DBNull.Value)
                {
                    txtFechaFin.Text = "";
                }
                else
                {
                    txtFechaFin.Text = Convert.ToDateTime(f["FechaFin"]).ToString("yyyy-MM-dd HH:mm");
                }

                lblMensaje.Text = "Detalle cargado. Modifique y presione Guardar.";
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
    protected void gvDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
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
        string mensaje = negocio.EliminarDetalle(id);

        if (mensaje == "")
        {
            lblMensaje.Text = "Detalle eliminado!";
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
        txtDescripcion.Text = "";
        txtFechaInicio.Text = "";
        txtFechaFin.Text = "";
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
