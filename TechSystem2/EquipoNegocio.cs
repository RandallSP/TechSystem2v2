using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de los equipos
// Aqui estan las reglas del sistema (validaciones, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class EquipoNegocio
{
    // capa de datos
    EquipoDatos datos = new EquipoDatos();

    // devuelve todos los equipos
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarEquipos()
    {
        try
        {
            return datos.ListarTodos();
        }
        catch (Exception)
        {
            return null;
        }
    }

    // busca equipos por tipo, modelo o usuario
    public DataTable BuscarEquipos(string texto)
    {
        try
        {
            return datos.Buscar(texto);
        }
        catch (Exception)
        {
            return null;
        }
    }

    // obtiene un solo equipo por su ID
    public DataTable ObtenerEquipo(int id)
    {
        try
        {
            return datos.ObtenerPorId(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    // devuelve los usuarios para llenar el combo de la pagina
    public DataTable ListarUsuariosParaCombo()
    {
        try
        {
            return datos.ListarUsuarios();
        }
        catch (Exception)
        {
            return null;
        }
    }

    // guarda un equipo nuevo o actualiza uno que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarEquipo(string id, string tipo, string modelo, string idUsuario)
    {
        // ---- reglas de negocio (validaciones) ----
        if (tipo.Trim() == "")
        {
            return "Escriba el tipo de equipo";
        }
        if (modelo.Trim() == "")
        {
            return "Escriba el modelo del equipo";
        }
        if (idUsuario == "")
        {
            return "Seleccione un usuario";
        }

        try
        {
            int idUsuarioNumero = Convert.ToInt32(idUsuario);

            if (id == "")
            {
                datos.Insertar(tipo.Trim(), modelo.Trim(), idUsuarioNumero);
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, tipo.Trim(), modelo.Trim(), idUsuarioNumero);
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina un equipo
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarEquipo(int id)
    {
        try
        {
            datos.Eliminar(id);
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al eliminar. Intente de nuevo.";
        }
    }
}

}
