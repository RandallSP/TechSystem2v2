using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de las reparaciones
// Aqui estan las reglas del sistema (validaciones, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class ReparacionNegocio
{
    // capa de datos
    ReparacionDatos datos = new ReparacionDatos();

    // devuelve todas las reparaciones
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarReparaciones()
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

    // busca reparaciones por estado, equipo o usuario
    public DataTable BuscarReparaciones(string texto)
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

    // obtiene una sola reparacion por su ID
    public DataTable ObtenerReparacion(int id)
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

    // devuelve los equipos para llenar el combo de la pagina
    public DataTable ListarEquiposParaCombo()
    {
        try
        {
            return datos.ListarEquipos();
        }
        catch (Exception)
        {
            return null;
        }
    }

    // guarda una reparacion nueva o actualiza una que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarReparacion(string id, string idEquipo, string estado)
    {
        // ---- reglas de negocio (validaciones) ----
        if (idEquipo == "")
        {
            return "Seleccione un equipo";
        }
        if (estado == "")
        {
            return "Seleccione un estado";
        }

        try
        {
            int idEquipoNumero = Convert.ToInt32(idEquipo);

            if (id == "")
            {
                datos.Insertar(idEquipoNumero, estado);
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, idEquipoNumero, estado);
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina una reparacion
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarReparacion(int id)
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
