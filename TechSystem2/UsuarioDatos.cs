using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

// Capa de datos de los usuarios
// Aqui van las consultas a la base de datos
// Todas las consultas son procedimientos almacenados
public class UsuarioDatos
{
    // cadena de conexion a la base de datos
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    // devuelve todos los usuarios
    // usamos using para que la conexion se cierre sola, aunque haya un error
    public DataTable ListarTodos()
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Listar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // busca usuarios por nombre o correo
    public DataTable Buscar(string texto)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Buscar", con))
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

    // obtiene un solo usuario por su ID
    public DataTable ObtenerPorId(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_ObtenerPorId", con))
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

    // busca un usuario por correo y clave (para el login)
    // la clave ya llega encriptada desde la capa de negocio
    public DataTable ObtenerPorLogin(string correo, string claveEncriptada)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Login", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@clave", claveEncriptada);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }

    // inserta un nuevo usuario
    // la clave llega ya encriptada
    public void Insertar(string nombre, string correo, string telefono, string claveEncriptada)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Insertar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@clave", claveEncriptada);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // actualiza los datos de un usuario
    // si la clave llega vacia, no se cambia (eso lo decide el procedimiento)
    public void Actualizar(int id, string nombre, string correo, string telefono, string claveEncriptada)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Actualizar", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@clave", claveEncriptada);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    // elimina un usuario de la base de datos
    public void Eliminar(int id)
    {
        using (SqlConnection con = new SqlConnection(conexion))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Usuarios_Eliminar", con))
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
