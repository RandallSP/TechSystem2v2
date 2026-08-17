using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

// Capa de datos de las reparaciones
// Aqui van las consultas a la base de datos
// Todas las consultas son procedimientos almacenados
public class ReparacionDatos
{
    // cadena de conexion a la base de datos
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    // devuelve todas las reparaciones con el equipo y su usuario
    // usamos using para que la conexion se cierre sola, aunque haya un error
    public DataTable ListarTodos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_Listar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // busca reparaciones por estado, equipo o usuario
    public DataTable Buscar(string texto)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_Buscar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@texto", texto);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // obtiene una sola reparacion por su ID
    public DataTable ObtenerPorId(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_ObtenerPorId", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // devuelve los equipos para llenar el combo de la pagina
    public DataTable ListarEquipos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_ListarEquipos", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // inserta una reparacion nueva
    // la fecha de solicitud se llena sola en la base de datos (GETDATE)
    public void Insertar(int idEquipo, string estado)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_Insertar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipoId", idEquipo);
                cmd.Parameters.AddWithValue("@estado", estado);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // actualiza los datos de una reparacion
    public void Actualizar(int id, int idEquipo, string estado)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_Actualizar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@equipoId", idEquipo);
                cmd.Parameters.AddWithValue("@estado", estado);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // elimina una reparacion
    // sus detalles y asignaciones se borran solos (CASCADE en la base de datos)
    public void Eliminar(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Reparaciones_Eliminar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

}
