using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar los tecnicos del sistema
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (TecnicoNegocio)
public partial class Tecnicos : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected TextBox txtNombre;
    protected TextBox txtEspecialidad;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvTecnicos;

    // capa de negocio
    TecnicoNegocio negocio = new TecnicoNegocio();

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
            CargarTabla();
        }
    }

    // Carga todos los tecnicos en el grid
    public void CargarTabla()
    {
        DataTable dt = negocio.ListarTecnicos();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los datos.";
            return;
        }
        gvTecnicos.DataSource = dt;
        gvTecnicos.DataBind();
    }

    // Buscar tecnicos
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = negocio.BuscarTecnicos(texto);
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al buscar.";
            return;
        }
        gvTecnicos.DataSource = dt;
        gvTecnicos.DataBind();
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
        string mensaje = negocio.GuardarTecnico(id, txtNombre.Text, txtEspecialidad.Text);

        if (mensaje == "")
        {
            LimpiarFormulario();
            CargarTabla();
            if (id == "")
            {
                lblMensaje.Text = "Tecnico guardado!";
            }
            else
            {
                lblMensaje.Text = "Tecnico actualizado!";
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
    protected void gvTecnicos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = negocio.ObtenerTecnico(id);
            if (dt == null)
            {
                lblMensaje.Text = "Ocurrio un error al cargar el tecnico.";
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["TecnicoID"].ToString();
                txtNombre.Text = f["Nombre"].ToString();
                txtEspecialidad.Text = f["Especialidad"].ToString();
                lblMensaje.Text = "Tecnico cargado. Modifique y presione Guardar.";
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
    protected void gvTecnicos_RowDataBound(object sender, GridViewRowEventArgs e)
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
        string mensaje = negocio.EliminarTecnico(id);

        if (mensaje == "")
        {
            lblMensaje.Text = "Tecnico eliminado!";
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
        txtNombre.Text = "";
        txtEspecialidad.Text = "";
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
