using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

// Capa de datos de las asignaciones
// Aqui van las consultas a la base de datos
// Todas las consultas son procedimientos almacenados
public class AsignacionDatos
{
    // cadena de conexion a la base de datos
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    // devuelve todas las asignaciones con la reparacion, el equipo, el usuario y el tecnico
    // usamos using para que la conexion se cierre sola, aunque haya un error
    public DataTable ListarTodos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_Listar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // busca asignaciones por estado, equipo, usuario o tecnico
    public DataTable Buscar(string texto)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_Buscar", con))
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

    // obtiene una sola asignacion por su ID
    public DataTable ObtenerPorId(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_ObtenerPorId", con))
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
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_ListarReparaciones", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // devuelve los tecnicos para llenar el combo de la pagina
    public DataTable ListarTecnicos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_ListarTecnicos", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // inserta una asignacion nueva
    // la fecha de asignacion se llena sola en la base de datos (GETDATE)
    public void Insertar(int idReparacion, int idTecnico)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_Insertar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@reparacionId", idReparacion);
                cmd.Parameters.AddWithValue("@tecnicoId", idTecnico);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // actualiza los datos de una asignacion
    public void Actualizar(int id, int idReparacion, int idTecnico)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_Actualizar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@reparacionId", idReparacion);
                cmd.Parameters.AddWithValue("@tecnicoId", idTecnico);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // elimina una asignacion de la base de datos
    public void Eliminar(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Asignaciones_Eliminar", con))
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
