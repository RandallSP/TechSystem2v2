using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

public partial class Tecnicos : System.Web.UI.Page
{
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

    // capa de datos de tecnicos
    TecnicoDatos datos = new TecnicoDatos();

    protected void Page_Load(object sender, EventArgs e)
    {
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

    public void CargarTabla()
    {
        DataTable dt = datos.ListarTodos();
        gvTecnicos.DataSource = dt;
        gvTecnicos.DataBind();
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
        gvTecnicos.DataSource = dt;
        gvTecnicos.DataBind();
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
        if (txtNombre.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba el nombre del tecnico";
            return;
        }
        if (txtEspecialidad.Text.Trim() == "")
        {
            lblMensaje.Text = "Escriba la especialidad";
            return;
        }

        string nombre = txtNombre.Text.Trim();
        string especialidad = txtEspecialidad.Text.Trim();

        if (txtID.Text == "")
        {
            datos.Insertar(nombre, especialidad);
            lblMensaje.Text = "Tecnico guardado!";
        }
        else
        {
            int id = Convert.ToInt32(txtID.Text);
            datos.Actualizar(id, nombre, especialidad);
            lblMensaje.Text = "Tecnico actualizado!";
        }

        LimpiarFormulario();
        CargarTabla();
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    protected void gvTecnicos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Seleccionar")
        {
            DataTable dt = datos.ObtenerPorId(id);
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

    protected void btnSi_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(txtID.Text);
        datos.Eliminar(id);

        lblMensaje.Text = "Tecnico eliminado!";
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
        txtNombre.Text = "";
        txtEspecialidad.Text = "";
    }
}

}
