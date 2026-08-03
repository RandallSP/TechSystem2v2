using System;
using System.Data.SqlClient;

// Pagina de Login - Autenticacion de usuarios
namespace TechSystem2
{

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Cuando carga la pagina no hay que hacer nada especial
    }

    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        try
        {
            // Agarramos lo que el usuario escribio en las cajas de texto
            string correo = txtCorreo.Text.Trim();
            string clave = txtClave.Text.Trim();

            // Revisamos que no esten vacios los campos
            if (correo == "")
            {
                lblMensaje.Text = "Por favor escriba el correo electronico.";
                return;
            }

            if (clave == "")
            {
                lblMensaje.Text = "Por favor escriba la clave.";
                return;
            }

            // Conexion a la base de datos
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

            // Abrimos la conexion
            conn.Open();

            // Consulta para buscar el usuario (concatenamos el correo y clave)
            string query = "SELECT UsuarioID, Nombre FROM Usuarios WHERE CorreoElectronico = '" + correo + "' AND Clave = '" + clave + "'";

            SqlCommand cmd = new SqlCommand(query, conn);

            // Ejecutamos la consulta y leemos los resultados
            SqlDataReader lector = cmd.ExecuteReader();

            if (lector.Read() == true)
            {
                // Si encontro un registro, el usuario existe
                // Guardamos los datos en variables de sesion
                Session["UsuarioID"] = lector["UsuarioID"].ToString();
                Session["NombreUsuario"] = lector["Nombre"].ToString();

                // Cerramos el lector y la conexion
                lector.Close();
                conn.Close();

                // Redirigimos a la pagina principal del sistema
                Response.Redirect("Default.aspx");
            }
            else
            {
                // Si no encontro nada, la clave o el correo estan mal
                lblMensaje.Text = "Correo o clave incorrectos. Intente de nuevo.";

                // Cerramos todo
                lector.Close();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            // Si algo sale mal mostramos un mensaje de error
            lblMensaje.Text = "Error al conectar con la base de datos: " + ex.Message;
        }
    }
}

}
