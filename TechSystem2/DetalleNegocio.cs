using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de los detalles de reparacion
// Aqui estan las reglas del sistema (validaciones, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class DetalleNegocio
{
    // capa de datos
    DetalleDatos datos = new DetalleDatos();

    // devuelve todos los detalles
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarDetalles()
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

    // busca detalles por descripcion, estado, equipo o usuario
    public DataTable BuscarDetalles(string texto)
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

    // obtiene un solo detalle por su ID
    public DataTable ObtenerDetalle(int id)
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

    // guarda un detalle nuevo o actualiza uno que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarDetalle(string id, string idReparacion, string descripcion, string fechaInicio, string fechaFin)
    {
        // ---- reglas de negocio (validaciones) ----
        if (idReparacion == "")
        {
            return "Seleccione una reparacion";
        }
        if (descripcion.Trim() == "")
        {
            return "Escriba la descripcion";
        }

        // convertimos las fechas: si vienen vacias, se guardan como null
        DateTime? fechaInicioValor = null;
        if (fechaInicio.Trim() != "")
        {
            DateTime fechaTemporalInicio;
            if (!DateTime.TryParse(fechaInicio.Trim(), out fechaTemporalInicio))
            {
                return "La fecha de inicio no es valida (ej: 2026-07-16 10:15)";
            }
            fechaInicioValor = fechaTemporalInicio;
        }

        DateTime? fechaFinValor = null;
        if (fechaFin.Trim() != "")
        {
            DateTime fechaTemporalFin;
            if (!DateTime.TryParse(fechaFin.Trim(), out fechaTemporalFin))
            {
                return "La fecha de fin no es valida (ej: 2026-07-16 10:15)";
            }
            fechaFinValor = fechaTemporalFin;
        }

        // la fecha de fin no puede ser antes que la fecha de inicio
        if (fechaInicioValor != null && fechaFinValor != null && fechaFinValor < fechaInicioValor)
        {
            return "La fecha de fin no puede ser antes que la fecha de inicio";
        }

        try
        {
            int idReparacionNumero = Convert.ToInt32(idReparacion);

            if (id == "")
            {
                datos.Insertar(idReparacionNumero, descripcion.Trim(), fechaInicioValor, fechaFinValor);
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, idReparacionNumero, descripcion.Trim(), fechaInicioValor, fechaFinValor);
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina un detalle
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarDetalle(int id)
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
