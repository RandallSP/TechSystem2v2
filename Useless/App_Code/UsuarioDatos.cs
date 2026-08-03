using System; // por si acaso
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Clase que maneja todas las operaciones de la tabla Usuarios.
/// Capa de datos: aqui van los SELECT, INSERT, UPDATE y DELETE.
/// </summary>
public class UsuarioDatos
{
    private ConexionDB conexionDB;

    public UsuarioDatos()
    {
        conexionDB = new ConexionDB();
    }

    /// <summary>
    /// Obtiene todos los usuarios de la tabla Usuarios.
    /// </summary>
    public DataTable ListarTodos()
    {
        DataTable tabla = new DataTable();

        try
        {
            // consulta simple: trae todos los usuarios
            string consulta = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono FROM Usuarios ORDER BY Nombre";

            conexionDB.Abrir();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionDB.ObtenerConexion());
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al listar los usuarios: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Busca usuarios por nombre usando WHERE y LIKE.
    /// Filtro: busca coincidencias parciales en el nombre.
    /// </summary>
    public DataTable Buscar(string texto)
    {
        DataTable tabla = new DataTable();

        try
        {
            // consulta con WHERE y LIKE para el filtro de busqueda
            string consulta = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono " +
                              "FROM Usuarios " +
                              "WHERE Nombre LIKE @Texto OR CorreoElectronico LIKE @Texto " +
                              "ORDER BY Nombre";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@Texto", "%" + texto + "%");
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al buscar usuarios: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Obtiene un usuario por su ID (para cargarlo en el formulario de edicion).
    /// </summary>
    public DataTable ObtenerPorId(int usuarioID)
    {
        DataTable tabla = new DataTable();

        try
        {
            string consulta = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono " +
                              "FROM Usuarios WHERE UsuarioID = @UsuarioID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el usuario: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Inserta un nuevo usuario en la base de datos.
    /// </summary>
    public void Insertar(string nombre, string correo, string telefono)
    {
        try
        {
            string consulta = "INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono) " +
                              "VALUES (@Nombre, @Correo, @Telefono)";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Correo", correo);
            comando.Parameters.AddWithValue("@Telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar el usuario: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Actualiza un usuario existente en la base de datos.
    /// </summary>
    public void Actualizar(int usuarioID, string nombre, string correo, string telefono)
    {
        try
        {
            string consulta = "UPDATE Usuarios SET Nombre = @Nombre, CorreoElectronico = @Correo, " +
                              "Telefono = @Telefono WHERE UsuarioID = @UsuarioID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Correo", correo);
            comando.Parameters.AddWithValue("@Telefono", string.IsNullOrEmpty(telefono) ? (object)DBNull.Value : telefono);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el usuario: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Elimina un usuario de la base de datos por su ID.
    /// </summary>
    public void Eliminar(int usuarioID)
    {
        try
        {
            string consulta = "DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el usuario: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }
}
