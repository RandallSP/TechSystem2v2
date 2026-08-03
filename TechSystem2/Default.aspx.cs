using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina principal del sistema, aqui tambien esta el login
public partial class Default : System.Web.UI.Page
{
    // Los controles que usamos en la pagina
    protected Panel pnlLogin;
    protected Panel pnlSistema;
    protected TextBox txtCorreo;
    protected TextBox txtClave;
    protected Button btnEntrar;
    protected Label lblMensaje;
    protected Label lblNombre;
    protected Button btnSalir;

    // Lo primero que pasa cuando se carga la pagina
    protected void Page_Load(object sender, EventArgs e)
    {
        // Miramos si ya inicio sesion o no
        if (Session["nombre"] == null)
        {
            // Si no ha entrado, mostramos el login
            pnlLogin.Visible = true;
            pnlSistema.Visible = false;
        }
        else
        {
            // Si ya entro, mostramos el sistema y su nombre
            pnlLogin.Visible = false;
            pnlSistema.Visible = true;
            lblNombre.Text = "Hola, " + Session["nombre"].ToString();
        }
    }

    // click en el boton Entrar del login
    protected void btnEntrar_Click(object sender, EventArgs e)
    {
        // agarramos lo que escribio
        string correo = txtCorreo.Text;
        string clave = txtClave.Text;

        // validamos que no este vacio (validacion basica)
        if (correo == "")
        {
            lblMensaje.Text = "Escriba el correo por favor";
            return;
        }
        if (clave == "")
        {
            lblMensaje.Text = "Escriba la clave por favor";
            return;
        }

        try
        {
            // nos conectamos a la base de datos
            SqlConnection con = new SqlConnection();
            con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
            con.Open();

            // buscamos el usuario con ese correo y clave
            // concatenamos directo el sql (no usamos parametros porque aun no los entiendo bien)
            string sql = "SELECT * FROM Usuarios WHERE CorreoElectronico = '" + correo + "' AND Clave = '" + clave + "'";
            SqlCommand cmd = new SqlCommand(sql, con);

            // leemos lo que nos devuelve la consulta
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                // el usuario si existe, guardamos sus datos
                Session["id"] = dr["UsuarioID"].ToString();
                Session["nombre"] = dr["Nombre"].ToString();

                dr.Close();
                con.Close();

                // recargamos la pagina para que muestre el sistema
                Response.Redirect("Default.aspx");
            }
            else
            {
                // no se encontro, mostramos mensaje de error
                lblMensaje.Text = "Correo o clave incorrectos";

                dr.Close();
                con.Close();
            }
        }
        catch (Exception ex)
        {
            // cualquier error lo mostramos
            lblMensaje.Text = "Error: " + ex.Message;
        }
    }

    // click en el boton Salir
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        // borramos lo que guardamos en la sesion
        Session["id"] = null;
        Session["nombre"] = null;

        // volvemos a cargar la pagina (mostrara el login)
        Response.Redirect("Default.aspx");
    }
}

}
