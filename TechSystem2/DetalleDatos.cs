using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

// Capa de datos de los detalles de reparacion
// Aqui van las consultas a la base de datos
// Todas las consultas son procedimientos almacenados
public class DetalleDatos
{
    // cadena de conexion a la base de datos
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    // devuelve todos los detalles con la reparacion, el equipo y el usuario
    // usamos using para que la conexion se cierre sola, aunque haya un error
    public DataTable ListarTodos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_Listar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // busca detalles por descripcion, estado, equipo o usuario
    public DataTable Buscar(string texto)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_Buscar", con))
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

    // obtiene un solo detalle por su ID
    public DataTable ObtenerPorId(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_ObtenerPorId", con))
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

    // devuelve las reparaciones para llenar el combo de la pagina
    public DataTable ListarReparaciones()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_ListarReparaciones", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // inserta un detalle nuevo
    // las fechas pueden ser null (si no tienen fecha)
    public void Insertar(int idReparacion, string descripcion, DateTime? fechaInicio, DateTime? fechaFin)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_Insertar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@reparacionId", idReparacion);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                if (fechaInicio == null)
                    cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
                if (fechaFin == null)
                    cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // actualiza los datos de un detalle
    public void Actualizar(int id, int idReparacion, string descripcion, DateTime? fechaInicio, DateTime? fechaFin)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_Actualizar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@reparacionId", idReparacion);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                if (fechaInicio == null)
                    cmd.Parameters.AddWithValue("@fechaInicio", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio.Value);
                if (fechaFin == null)
                    cmd.Parameters.AddWithValue("@fechaFin", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin.Value);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // elimina un detalle de la base de datos
    public void Eliminar(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Detalles_Eliminar", con))
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
