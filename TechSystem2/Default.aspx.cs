using System;
using System.Data;
using System.Web.UI.WebControls;

namespace TechSystem2
{

// Pagina principal del sistema, aqui tambien esta el login
// Esta pagina es la capa de presentacion: solo muestra datos
// y llama a la capa de negocio (UsuarioNegocio)
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
        string correo = txtCorreo.Text.Trim();
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

        // mandamos los datos a la capa de negocio (ahi se encripta la clave)
        UsuarioNegocio negocio = new UsuarioNegocio();
        DataTable dt = negocio.IniciarSesion(correo, clave);

        if (dt == null)
        {
            // hubo un problema con la base de datos
            lblMensaje.Text = "Ocurrio un error al conectar con la base de datos. Intente de nuevo.";
        }
        else if (dt.Rows.Count > 0)
        {
            // el usuario existe, guardamos sus datos en la sesion
            DataRow fila = dt.Rows[0];
            Session["id"] = fila["UsuarioID"].ToString();
            Session["nombre"] = fila["Nombre"].ToString();

            // recargamos la pagina para que muestre el sistema
            Response.Redirect("Default.aspx");
        }
        else
        {
            // no se encontro, mostramos mensaje de error
            lblMensaje.Text = "Correo o clave incorrectos";
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
