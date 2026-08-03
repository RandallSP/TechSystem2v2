using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina para administrar los usuarios del sistema
public partial class Usuarios : System.Web.UI.Page
{
    // los controles de la pagina
    protected TextBox txtBuscar;
    protected Button btnBuscar;
    protected Button btnLimpiar;
    protected TextBox txtID;
    protected TextBox txtNombre;
    protected TextBox txtCorreo;
    protected TextBox txtTelefono;
    protected TextBox txtClave;
    protected Button btnGuardar;
    protected Button btnNuevo;
    protected Label lblMensaje;
    protected Panel pnlConfirmar;
    protected Button btnSi;
    protected Button btnNo;
    protected GridView gvUsuarios;

    // creamos el objeto de la capa de datos
    UsuarioDatos datos = new UsuarioDatos();

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

    // Carga todos los usuarios en el grid
    public void CargarTabla()
    {
        DataTable dt = datos.ListarTodos();
        gvUsuarios.DataSource = dt;
        gvUsuarios.DataBind();
    }

    // Buscar usuarios
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        string texto = txtBuscar.Text.Trim();
        if (texto == "")
        {
            CargarTabla();
            lblMensaje.Text = "";
            return;
        }

        DataTable dt = datos.Buscar(texto);
        gvUsuarios.DataSource = dt;
        gvUsuarios.DataBind();
        lblMensaje.Text = "Se encontraron " + dt.Rows.Count + " resultados";
    }

    // Limpiar busqueda
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtBuscar.Text = "";
        CargarTabla();
        lblMensaje.Text = "";
    }

    // Guardar o actualizar
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (txtNombre.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba el nombre del usuario";
            return;
        }
        if (txtCorreo.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba el correo del usuario";
            return;
        }

        string nombre = txtNombre.Text.Trim();
        string correo = txtCorreo.Text.Trim();
        string telefono = txtTelefono.Text.Trim();
        string clave = txtClave.Text.Trim();

        if (txtID.Text == "")
        {
            datos.Insertar(nombre, correo, telefono, clave);
            lblMensaje.Text = "Usuario guardado!";
        }
        else
        {
            int id = Convert.ToInt32(txtID.Text);
            datos.Actualizar(id, nombre, correo, telefono, clave);
            lblMensaje.Text = "Usuario actualizado!";
        }

        LimpiarFormulario();
        CargarTabla();
    }

    // Boton nuevo
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    // Eventos del grid (Seleccionar y Eliminar)
    protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = datos.ObtenerPorId(id);

            if (dt.Rows.Count > 0)
            {
                DataRow f = dt.Rows[0];
                txtID.Text = f["UsuarioID"].ToString();
                txtNombre.Text = f["Nombre"].ToString();
                txtCorreo.Text = f["CorreoElectronico"].ToString();
                txtTelefono.Text = f["Telefono"].ToString();
                txtClave.Text = f["Clave"].ToString();
                lblMensaje.Text = "Usuario cargado. Modifique y presione Guardar.";
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
    protected void gvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
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
        datos.Eliminar(id);

        lblMensaje.Text = "Usuario eliminado!";
        pnlConfirmar.Visible = false;
        LimpiarFormulario();
        CargarTabla();
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
        txtCorreo.Text = "";
        txtTelefono.Text = "";
        txtClave.Text = "";
    }
}

}
