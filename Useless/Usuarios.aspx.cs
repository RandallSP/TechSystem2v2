using System; // por si acaso
using System.Data;

/// <summary>
/// Pagina de mantenimiento de Usuarios (CRUD completo).
/// Permite agregar, consultar, modificar y eliminar usuarios.
/// Incluye filtro de busqueda con WHERE.
/// </summary>
public partial class Usuarios : System.Web.UI.Page
{
    // creamos la capa de datos para usar en toda la pagina
    private UsuarioDatos usuarioDatos = new UsuarioDatos();

    /// <summary>
    /// Evento que se ejecuta cuando la pagina se carga.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Verificamos que el usuario haya iniciado sesion
        if (Session["NombreUsuario"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        // importante: solo cargamos la tabla la primera vez que entra a la pagina
        // si no hacemos esto, se pierden los datos del formulario despues de hacer consultas
        if (!IsPostBack)
        {
            LlenarGrid();
        }
    }

    /// <summary>
    /// Llena el GridView con todos los usuarios de la base de datos.
    /// </summary>
    private void LlenarGrid()
    {
        try
        {
            DataTable tabla = usuarioDatos.ListarTodos();
            gvUsuarios.DataSource = tabla;
            gvUsuarios.DataBind();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    /// <summary>
    /// Boton Buscar: filtra usuarios por nombre o correo usando WHERE y LIKE.
    /// </summary>
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            string texto = txtBuscarUsuario.Text.Trim();

            if (string.IsNullOrEmpty(texto))
            {
                // si no escribieron nada, mostramos todos
                LlenarGrid();
                lblMensaje.Text = "";
            }
            else
            {
                // aplicamos el filtro con WHERE
                DataTable tabla = usuarioDatos.Buscar(texto);
                gvUsuarios.DataSource = tabla;
                gvUsuarios.DataBind();
                lblMensaje.Text = "Resultados encontrados: " + tabla.Rows.Count;
                lblMensaje.CssClass = "mensaje-exito";
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al buscar: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    /// <summary>
    /// Boton Limpiar: limpia el filtro y vuelve a mostrar todos los usuarios.
    /// </summary>
    protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
    {
        txtBuscarUsuario.Text = "";
        LlenarGrid();
        lblMensaje.Text = "";
    }

    /// <summary>
    /// Boton Guardar: sirve tanto para insertar como para actualizar un usuario.
    /// Si hay ID guarda en el HiddenField, actualiza. Si no, inserta.
    /// </summary>
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            // validamos que los campos obligatorios no esten vacios
            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba el nombre del usuario.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }

            if (string.IsNullOrEmpty(txtCorreo.Text.Trim()))
            {
                lblMensaje.Text = "Por favor escriba el correo electronico.";
                lblMensaje.CssClass = "mensaje-error";
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrEmpty(hfUsuarioID.Value))
            {
                // no hay ID = es un usuario nuevo, lo insertamos
                usuarioDatos.Insertar(nombre, correo, telefono);
                lblMensaje.Text = "Usuario guardado exitosamente.";
            }
            else
            {
                // si hay ID = es una actualizacion
                int usuarioID = Convert.ToInt32(hfUsuarioID.Value);
                usuarioDatos.Actualizar(usuarioID, nombre, correo, telefono);
                lblMensaje.Text = "Usuario actualizado exitosamente.";
            }

            lblMensaje.CssClass = "mensaje-exito";

            // limpiamos el formulario despues de guardar
            LimpiarFormulario();

            // refrescamos la tabla
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al guardar el usuario: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    /// <summary>
    /// Boton Nuevo: limpia el formulario para agregar un usuario desde cero.
    /// </summary>
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        lblMensaje.Text = "";
    }

    /// <summary>
    /// Evento que maneja los comandos del GridView (Seleccionar y Eliminar).
    /// </summary>
    protected void gvUsuarios_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            int usuarioID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Seleccionar")
            {
                // cargar el usuario seleccionado en el formulario para editar
                DataTable tabla = usuarioDatos.ObtenerPorId(usuarioID);

                if (tabla.Rows.Count > 0)
                {
                    DataRow fila = tabla.Rows[0];
                    hfUsuarioID.Value = fila["UsuarioID"].ToString();
                    txtNombre.Text = fila["Nombre"].ToString();
                    txtCorreo.Text = fila["CorreoElectronico"].ToString();
                    txtTelefono.Text = fila["Telefono"] != DBNull.Value ? fila["Telefono"].ToString() : "";

                    lblMensaje.Text = "Usuario cargado. Modifique los datos y presione Guardar.";
                    lblMensaje.CssClass = "mensaje-exito";
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                // mostrar panel de confirmacion para eliminar
                hfUsuarioID.Value = usuarioID.ToString();
                pnlConfirmacion.Visible = true;
                lblMensaje.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
        }
    }

    /// <summary>
    /// Se ejecuta al enlazar cada fila del GridView.
    /// Lo usamos para ponerle estilo a los botones de la columna Acciones.
    /// </summary>
    protected void gvUsuarios_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkSeleccionar = (LinkButton)e.Row.FindControl("lnkSeleccionar");
            LinkButton lnkEliminar = (LinkButton)e.Row.FindControl("lnkEliminar");

            if (lnkSeleccionar != null)
            {
                lnkSeleccionar.CssClass = "btn-accion";
            }
            if (lnkEliminar != null)
            {
                lnkEliminar.CssClass = "btn-eliminar";
            }
        }
    }

    /// <summary>
    /// Boton confirmar eliminacion: elimina el usuario de la base de datos.
    /// </summary>
    protected void btnSiEliminar_Click(object sender, EventArgs e)
    {
        try
        {
            int usuarioID = Convert.ToInt32(hfUsuarioID.Value);
            usuarioDatos.Eliminar(usuarioID);

            lblMensaje.Text = "Usuario eliminado exitosamente.";
            lblMensaje.CssClass = "mensaje-exito";

            // escondemos el panel de confirmacion y limpiamos
            pnlConfirmacion.Visible = false;
            LimpiarFormulario();
            LlenarGrid();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = "Error al eliminar el usuario: " + ex.Message;
            lblMensaje.CssClass = "mensaje-error";
            pnlConfirmacion.Visible = false;
        }
    }

    /// <summary>
    /// Boton cancelar eliminacion: esconde el panel de confirmacion.
    /// </summary>
    protected void btnNoCancelar_Click(object sender, EventArgs e)
    {
        pnlConfirmacion.Visible = false;
        lblMensaje.Text = "";
    }

    /// <summary>
    /// Limpia todos los campos del formulario y el ID oculto.
    /// </summary>
    private void LimpiarFormulario()
    {
        hfUsuarioID.Value = "";
        txtNombre.Text = "";
        txtCorreo.Text = "";
        txtTelefono.Text = "";
    }
}
