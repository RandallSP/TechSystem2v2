using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de las asignaciones
// Aqui estan las reglas del sistema (validaciones, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class AsignacionNegocio
{
    // capa de datos
    AsignacionDatos datos = new AsignacionDatos();

    // devuelve todas las asignaciones
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarAsignaciones()
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

    // busca asignaciones por estado, equipo, usuario o tecnico
    public DataTable BuscarAsignaciones(string texto)
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

    // obtiene una sola asignacion por su ID
    public DataTable ObtenerAsignacion(int id)
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

    // devuelve las reparaciones para llenar el combo de la pagina
    public DataTable ListarReparacionesParaCombo()
    {
        try
        {
            return datos.ListarReparaciones();
        }
        catch (Exception)
        {
            return null;
        }
    }

    // devuelve los tecnicos para llenar el combo de la pagina
    public DataTable ListarTecnicosParaCombo()
    {
        try
        {
            return datos.ListarTecnicos();
        }
        catch (Exception)
        {
            return null;
        }
    }

    // guarda una asignacion nueva o actualiza una que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarAsignacion(string id, string idReparacion, string idTecnico)
    {
        // ---- reglas de negocio (validaciones) ----
        if (idReparacion == "")
        {
            return "Seleccione una reparacion";
        }
        if (idTecnico == "")
        {
            return "Seleccione un tecnico";
        }

        try
        {
            int idReparacionNumero = Convert.ToInt32(idReparacion);
            int idTecnicoNumero = Convert.ToInt32(idTecnico);

            if (id == "")
            {
                datos.Insertar(idReparacionNumero, idTecnicoNumero);
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, idReparacionNumero, idTecnicoNumero);
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina una asignacion
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarAsignacion(int id)
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
