using LOGIN.CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LOGIN.Capavistas
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

      
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            cls_Usuario.email = txtusuario.Text;
            cls_Usuario.clave = txtclave.Text;

            int resultado = CapaLogica.usuario.validausuario(cls_Usuario.email, cls_Usuario.clave);  // 1 si existe y 0 si no existe

            if (resultado == 0)
            {
            }
            else
            {
                Response.Redirect("home.aspx");
            }

        }
    }
}