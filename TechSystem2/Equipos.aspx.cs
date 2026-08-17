using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar los equipos del sistema
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (EquipoNegocio)
public partial class Equipos : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected TextBox txtTipo;
    protected TextBox txtModelo;
    protected DropDownList ddlUsuario;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvEquipos;

    // capa de negocio
    EquipoNegocio negocio = new EquipoNegocio();

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
            CargarUsuarios();
            CargarTabla();
        }
    }

    // llena el combo con los usuarios
    public void CargarUsuarios()
    {
        DataTable dt = negocio.ListarUsuariosParaCombo();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los usuarios.";
            return;
        }
        ddlUsuario.DataSource = dt;
        ddlUsuario.DataTextField = "Nombre";
        ddlUsuario.DataValueField = "UsuarioID";
        ddlUsuario.DataBind();
        ddlUsuario.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }

    // Carga todos los equipos en el grid
    public void CargarTabla()
    {
        DataTable dt = negocio.ListarEquipos();
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al cargar los datos.";
            return;
        }
        gvEquipos.DataSource = dt;
        gvEquipos.DataBind();
    }

    // Buscar equipos
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = negocio.BuscarEquipos(texto);
        if (dt == null)
        {
            lblMensaje.Text = "Ocurrio un error al buscar.";
            return;
        }
        gvEquipos.DataSource = dt;
        gvEquipos.DataBind();
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
        string mensaje = negocio.GuardarEquipo(id, txtTipo.Text, txtModelo.Text, ddlUsuario.SelectedValue);

        if (mensaje == "")
        {
            LimpiarFormulario();
            CargarTabla();
            if (id == "")
            {
                lblMensaje.Text = "Equipo guardado!";
            }
            else
            {
                lblMensaje.Text = "Equipo actualizado!";
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
    protected void gvEquipos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = negocio.ObtenerEquipo(id);
            if (dt == null)
            {
                lblMensaje.Text = "Ocurrio un error al cargar el equipo.";
                return;
            }

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["EquipoID"].ToString();
                txtTipo.Text = f["TipoEquipo"].ToString();
                txtModelo.Text = f["Modelo"].ToString();
                ddlUsuario.SelectedValue = f["UsuarioID"].ToString();
                lblMensaje.Text = "Equipo cargado. Modifique y presione Guardar.";
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
    protected void gvEquipos_RowDataBound(object sender, GridViewRowEventArgs e)
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
        string mensaje = negocio.EliminarEquipo(id);

        if (mensaje == "")
        {
            lblMensaje.Text = "Equipo eliminado!";
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
        txtTipo.Text = "";
        txtModelo.Text = "";
        if (ddlUsuario.Items.Count > 0) ddlUsuario.SelectedIndex = 0;
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
