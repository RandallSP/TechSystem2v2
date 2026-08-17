using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de los tecnicos
// Aqui estan las reglas del sistema (validaciones, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class TecnicoNegocio
{
    // capa de datos
    TecnicoDatos datos = new TecnicoDatos();

    // devuelve todos los tecnicos
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarTecnicos()
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

    // busca tecnicos por nombre o especialidad
    public DataTable BuscarTecnicos(string texto)
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

    // obtiene un solo tecnico por su ID
    public DataTable ObtenerTecnico(int id)
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

    // guarda un tecnico nuevo o actualiza uno que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarTecnico(string id, string nombre, string especialidad)
    {
        // ---- reglas de negocio (validaciones) ----
        if (nombre.Trim() == "")
        {
            return "Escriba el nombre del tecnico";
        }
        if (especialidad.Trim() == "")
        {
            return "Escriba la especialidad";
        }

        try
        {
            if (id == "")
            {
                datos.Insertar(nombre.Trim(), especialidad.Trim());
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, nombre.Trim(), especialidad.Trim());
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina un tecnico
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarTecnico(int id)
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
