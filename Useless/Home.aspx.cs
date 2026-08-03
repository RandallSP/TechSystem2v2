using System;

// Pagina de bienvenida - Se muestra despues de iniciar sesion
namespace TechSystem2
{

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Verificamos que el usuario haya iniciado sesion
            if (Session["NombreUsuario"] == null)
            {
                // Si no ha iniciado sesion lo mandamos al login
                Response.Redirect("Login.aspx");
                return;
            }

            // Mostramos el nombre del usuario en la pagina
            string nombre = Session["NombreUsuario"].ToString();
            lblBienvenida.Text = "Bienvenido, " + nombre + "!";
            lblUsuarioNav.Text = nombre;
        }
        catch (Exception ex)
        {
            // Si algo falla mostramos un mensaje
            lblBienvenida.Text = "Error al cargar los datos del usuario";
        }
    }

    protected void btnCerrarSesion_Click(object sender, EventArgs e)
    {
        // Limpiamos las variables de sesion
        Session["UsuarioID"] = null;
        Session["NombreUsuario"] = null;

        // Redirigimos al login
        Response.Redirect("Login.aspx");
    }
}

}
