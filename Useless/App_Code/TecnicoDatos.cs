using System; // por si acaso
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Clase que maneja todas las operaciones de la tabla Tecnicos.
/// Capa de datos: SELECT, INSERT, UPDATE y DELETE.
/// </summary>
public class TecnicoDatos
{
    private ConexionDB conexionDB;

    public TecnicoDatos()
    {
        conexionDB = new ConexionDB();
    }

    /// <summary>
    /// Obtiene todos los tecnicos de la tabla Tecnicos.
    /// </summary>
    public DataTable ListarTodos()
    {
        DataTable tabla = new DataTable();

        try
        {
            string consulta = "SELECT TecnicoID, Nombre, Especialidad FROM Tecnicos ORDER BY Nombre";

            conexionDB.Abrir();
            SqlDataAdapter adaptador = new SqlDataAdapter(consulta, conexionDB.ObtenerConexion());
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al listar los tecnicos: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Busca tecnicos por nombre o especialidad usando WHERE y LIKE.
    /// </summary>
    public DataTable Buscar(string texto)
    {
        DataTable tabla = new DataTable();

        try
        {
            // consulta con WHERE y LIKE para filtrar por nombre o especialidad
            string consulta = "SELECT TecnicoID, Nombre, Especialidad " +
                              "FROM Tecnicos " +
                              "WHERE Nombre LIKE @Texto OR Especialidad LIKE @Texto " +
                              "ORDER BY Nombre";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@Texto", "%" + texto + "%");
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al buscar tecnicos: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Obtiene un tecnico por su ID (para cargarlo en el formulario de edicion).
    /// </summary>
    public DataTable ObtenerPorId(int tecnicoID)
    {
        DataTable tabla = new DataTable();

        try
        {
            string consulta = "SELECT TecnicoID, Nombre, Especialidad " +
                              "FROM Tecnicos WHERE TecnicoID = @TecnicoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@TecnicoID", tecnicoID);
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            adaptador.Fill(tabla);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el tecnico: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }

        return tabla;
    }

    /// <summary>
    /// Inserta un nuevo tecnico en la base de datos.
    /// </summary>
    public void Insertar(string nombre, string especialidad)
    {
        try
        {
            string consulta = "INSERT INTO Tecnicos (Nombre, Especialidad) " +
                              "VALUES (@Nombre, @Especialidad)";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Especialidad", especialidad);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al guardar el tecnico: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Actualiza un tecnico existente en la base de datos.
    /// </summary>
    public void Actualizar(int tecnicoID, string nombre, string especialidad)
    {
        try
        {
            string consulta = "UPDATE Tecnicos SET Nombre = @Nombre, Especialidad = @Especialidad " +
                              "WHERE TecnicoID = @TecnicoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@TecnicoID", tecnicoID);
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Especialidad", especialidad);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el tecnico: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }

    /// <summary>
    /// Elimina un tecnico de la base de datos por su ID.
    /// </summary>
    public void Eliminar(int tecnicoID)
    {
        try
        {
            string consulta = "DELETE FROM Tecnicos WHERE TecnicoID = @TecnicoID";

            conexionDB.Abrir();
            SqlCommand comando = new SqlCommand(consulta, conexionDB.ObtenerConexion());
            comando.Parameters.AddWithValue("@TecnicoID", tecnicoID);
            comando.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al eliminar el tecnico: " + ex.Message);
        }
        finally
        {
            conexionDB.Cerrar();
        }
    }
}
