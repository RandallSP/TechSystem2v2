using System; // por si acaso
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Clase que maneja todas las operaciones de la tabla Equipos.
/// Capa de datos: SELECT, INSERT, UPDATE y DELETE con la FK hacia Usuarios.
/// </summary>
public class EquipoDatos
{
    private ConexionDB conexionDB;

    public EquipoDatos()
    {
        conexionDB = new ConexionDB();
    }

    /// <summary>
    /// Obtiene todos los equipos con el nombre del usuario (JOIN con Usuarios).
    /// </summary>
    public DataTable ListarTodos()
    {
        DataTable tabla = new DataTable();

        try
        {
            // consulta con JOIN para mostrar el nombre del usuario en vez del ID
            string consulta = "SELECT E.EquipoID, E.TipoEquipo, E.Modelo, " +
                              "U.Nombre AS NombreUsuario, E.UsuarioID " +
                              "FROM Equipos E " +
                              "INNER JOIN Usuarios U ON E.UsuarioID = U.UsuarioID " +
                              "ORDER BY E.TipoEquipo";

            conexionDB.Abrir();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionDB.ObtenerConexion());
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al listar los equipos: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Busca equipos por tipo, modelo o nombre de usuario usando WHERE y LIKE.
    /// </summary>
    public DataTable Buscar(string texto)
    {
        DataTable tabla = new DataTable();

        try
        {
            // consulta con WHERE y LIKE para filtrar por tipo, modelo o usuario
            string consulta = "SELECT E.EquipoID, E.TipoEquipo, E.Modelo, " +
                              "U.Nombre AS NombreUsuario, E.UsuarioID " +
                              "FROM Equipos E " +
                              "INNER JOIN Usuarios U ON E.UsuarioID = U.UsuarioID " +
                              "WHERE E.TipoEquipo LIKE @Texto OR E.Modelo LIKE @Texto OR U.Nombre LIKE @Texto " +
                              "ORDER BY E.TipoEquipo";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@Texto", "%" + texto + "%");
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al buscar equipos: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Obtiene un equipo por su ID (para cargarlo en el formulario de edicion).
    /// </summary>
    public DataTable ObtenerPorId(int equipoID)
    {
        DataTable tabla = new DataTable();

        try
        {
            string consulta = "SELECT EquipoID, TipoEquipo, Modelo, UsuarioID " +
                              "FROM Equipos WHERE EquipoID = @EquipoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@EquipoID", equipoID);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el equipo: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Obtiene la lista de usuarios para el dropdown (solo ID y Nombre).
    /// </summary>
    public DataTable ListarUsuarios()
    {
        DataTable tabla = new DataTable();

        try
        {
            string consulta = "SELECT UsuarioID, Nombre FROM Usuarios ORDER BY Nombre";

            conexionDB.Abrir();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionDB.ObtenerConexion());
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al listar usuarios para el dropdown: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Inserta un nuevo equipo en la base de datos.
    /// </summary>
    public void Insertar(string tipoEquipo, string modelo, int usuarioID)
    {
        try
        {
            string consulta = "INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID) " +
                              "VALUES (@TipoEquipo, @Modelo, @UsuarioID)";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@TipoEquipo", tipoEquipo);
            comando.Parameters.AddWithValue("@Modelo", modelo);
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar el equipo: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Actualiza un equipo existente en la base de datos.
    /// </summary>
    public void Actualizar(int equipoID, string tipoEquipo, string modelo, int usuarioID)
    {
        try
        {
            string consulta = "UPDATE Equipos SET TipoEquipo = @TipoEquipo, Modelo = @Modelo, " +
                              "UsuarioID = @UsuarioID WHERE EquipoID = @EquipoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@EquipoID", equipoID);
            comando.Parameters.AddWithValue("@TipoEquipo", tipoEquipo);
            comando.Parameters.AddWithValue("@Modelo", modelo);
            comando.Parameters.AddWithValue("@UsuarioID", usuarioID);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el equipo: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Elimina un equipo de la base de datos por su ID.
    /// </summary>
    public void Eliminar(int equipoID)
    {
        try
        {
            string consulta = "DELETE FROM Equipos WHERE EquipoID = @EquipoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@EquipoID", equipoID);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el equipo: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }
}
