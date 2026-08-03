using System; // por si acaso

/// <summary>
/// Pagina de inicio del sistema TechSystem.
/// Muestra la bienvenida y el menu de navegacion.
/// </summary>
public partial class Default : System.Web.UI.Page
{
    /// <summary>
    /// Cuando la pagina se carga, revisamos que el usuario haya iniciado sesion.
    /// Si no, lo mandamos al login.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Verificamos que el usuario haya iniciado sesion
        if (Session["NombreUsuario"] == null)
        {
            // Si no ha iniciado sesion lo mandamos al login
            Response.Redirect("Login.aspx");
            return;
        }

        // Mostramos el nombre del usuario que inicio sesion
        string nombre = Session["NombreUsuario"].ToString();
        lblUsuario.Text = "Bienvenido, " + nombre + "!";
    }
}
