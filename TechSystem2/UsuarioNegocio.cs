using System;
using System.Data;

namespace TechSystem2
{

// Capa de negocio de los usuarios
// Aqui estan las reglas del sistema (validaciones, encriptar clave, etc.)
// Las paginas NO hablan con la base de datos, siempre pasan por esta capa
public class UsuarioNegocio
{
    // capa de datos
    UsuarioDatos datos = new UsuarioDatos();

    // clase para encriptar las claves
    Seguridad seguridad = new Seguridad();

    // devuelve todos los usuarios
    // si algo falla devuelve null y la pagina muestra el mensaje
    public DataTable ListarUsuarios()
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

    // busca usuarios por nombre o correo
    public DataTable BuscarUsuarios(string texto)
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

    // obtiene un solo usuario por su ID
    public DataTable ObtenerUsuario(int id)
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

    // revisa que el correo y la clave sean correctos (login)
    // devuelve los datos del usuario, o null si fallo la base de datos
    public DataTable IniciarSesion(string correo, string clave)
    {
        try
        {
            // encriptamos la clave que escribio el usuario
            string claveEncriptada = seguridad.ObtenerHash(clave.Trim());

            return datos.ObtenerPorLogin(correo, claveEncriptada);
        }
        catch (Exception)
        {
            return null;
        }
    }

    // guarda un usuario nuevo o actualiza uno que ya existe
    // devuelve "" si todo salio bien, o un mensaje si hay un error
    public string GuardarUsuario(string id, string nombre, string correo, string telefono, string clave)
    {
        // ---- reglas de negocio (validaciones) ----
        if (nombre.Trim() == "")
        {
            return "Escriba el nombre del usuario";
        }
        if (correo.Trim() == "")
        {
            return "Escriba el correo del usuario";
        }
        if (!correo.Contains("@") || !correo.Contains("."))
        {
            return "El correo no tiene un formato valido (ej: correo@algo.com)";
        }
        if (telefono.Trim() != "")
        {
            if (!EsNumero(telefono.Trim()) || telefono.Trim().Length < 8)
            {
                return "El telefono debe tener al menos 8 numeros";
            }
        }
        if (id == "" && clave.Trim() == "")
        {
            return "Escriba la clave del usuario";
        }
        if (clave.Trim() != "" && clave.Trim().Length < 6)
        {
            return "La clave debe tener al menos 6 caracteres";
        }

        // encriptamos la clave solo si escribio una
        // (si edita un usuario y deja la clave vacia, no se cambia)
        string claveEncriptada = "";
        if (clave.Trim() != "")
        {
            claveEncriptada = seguridad.ObtenerHash(clave.Trim());
        }

        try
        {
            if (id == "")
            {
                datos.Insertar(nombre.Trim(), correo.Trim(), telefono.Trim(), claveEncriptada);
            }
            else
            {
                int idNumero = Convert.ToInt32(id);
                datos.Actualizar(idNumero, nombre.Trim(), correo.Trim(), telefono.Trim(), claveEncriptada);
            }
            return "";
        }
        catch (Exception)
        {
            return "Ocurrio un error al guardar. Intente de nuevo.";
        }
    }

    // elimina un usuario
    // devuelve "" si salio bien, o un mensaje si fallo
    public string EliminarUsuario(int id)
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

    // metodo auxiliar: dice si un texto tiene solo numeros
    private bool EsNumero(string texto)
    {
        foreach (char letra in texto)
        {
            if (!char.IsDigit(letra))
            {
                return false;
            }
        }
        return true;
    }
}

}
