using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

public partial class Equipos : System.Web.UI.Page
{
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

    // capa de datos de equipos
    EquipoDatos datos = new EquipoDatos();

    protected void Page_Load(object sender, EventArgs e)
    {
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

    public void CargarUsuarios()
    {
        DataTable dt = datos.ListarUsuarios();
        ddlUsuario.DataSource = dt;
        ddlUsuario.DataTextField = "Nombre";
        ddlUsuario.DataValueField = "UsuarioID";
        ddlUsuario.DataBind();
        ddlUsuario.Items.Insert(0, new ListItem("-- Seleccione --", ""));
    }

    public void CargarTabla()
    {
        DataTable dt = datos.ListarTodos();
        gvEquipos.DataSource = dt;
        gvEquipos.DataBind();
    }

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
        gvEquipos.DataSource = dt;
        gvEquipos.DataBind();
        lblMensaje.Text = "Se encontraron " + dt.Rows.Count + " resultados";
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        txtBuscar.Text = "";
        CargarTabla();
        lblMensaje.Text = "";
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (txtTipo.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba el tipo de equipo";
            return;
        }
        if (txtModelo.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba el modelo del equipo";
            return;
        }
        if (ddlUsuario.SelectedValue == "")
        {
            lblMensaje.Text = "Seleccione un usuario";
            return;
        }

        string tipo = txtTipo.Text.Trim();
        string modelo = txtModelo.Text.Trim();
        int idUsuario = Convert.ToInt32(ddlUsuario.SelectedValue);

        if (txtID.Text == "")
        {
            datos.Insertar(tipo, modelo, idUsuario);
            lblMensaje.Text = "Equipo guardado!";
        }
        else
        {
            int id = Convert.ToInt32(txtID.Text);
            datos.Actualizar(id, tipo, modelo, idUsuario);
            lblMensaje.Text = "Equipo actualizado!";
        }

        LimpiarFormulario();
        CargarTabla();
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    protected void gvEquipos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = datos.ObtenerPorId(id);
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

    protected void btnSi_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txtID.Text);
        datos.Eliminar(id);

        lblMensaje.Text = "Equipo eliminado!";
        pnlConfirmar.Visible = false;
        LimpiarFormulario();
        CargarTabla();
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        pnlConfirmar.Visible = false;
        lblMensaje.Text = "";
    }

    public void LimpiarFormulario()
    {
        txtID.Text = "";
        txtTipo.Text = "";
        txtModelo.Text = "";
        if (ddlUsuario.Items.Count > 0) ddlUsuario.SelectedIndex = 0;
    }
}

}
