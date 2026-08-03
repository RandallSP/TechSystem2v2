using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

// Capa de datos para la tabla Usuarios
// Aqui van las consultas SQL directas a la base de datos
public class UsuarioDatos
{
    // cadena de conexion publica (el profe dijo que usemos public)
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    // devuelve todos los usuarios en un DataTable
    public DataTable ListarTodos()
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono, ISNULL(Clave,'') AS Clave FROM Usuarios ORDER BY Nombre";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    // busca usuarios por nombre o correo
    public DataTable Buscar(string texto)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono, ISNULL(Clave,'') AS Clave FROM Usuarios WHERE Nombre LIKE '%" + texto + "%' OR CorreoElectronico LIKE '%" + texto + "%' ORDER BY Nombre";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    // obtiene un solo usuario por su ID
    public DataTable ObtenerPorId(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT UsuarioID, Nombre, CorreoElectronico, Telefono, ISNULL(Clave,'') AS Clave FROM Usuarios WHERE UsuarioID = " + id;
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    // inserta un nuevo usuario en la base de datos
    public void Insertar(string nombre, string correo, string telefono, string clave)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "INSERT INTO Usuarios (Nombre, CorreoElectronico, Telefono, Clave) VALUES ('" + nombre + "', '" + correo + "', '" + telefono + "', '" + clave + "')";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    // actualiza los datos de un usuario existente
    public void Actualizar(int id, string nombre, string correo, string telefono, string clave)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "UPDATE Usuarios SET Nombre = '" + nombre + "', CorreoElectronico = '" + correo + "', Telefono = '" + telefono + "', Clave = '" + clave + "' WHERE UsuarioID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    // elimina un usuario de la base de datos
    public void Eliminar(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "DELETE FROM Usuarios WHERE UsuarioID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }
}

}
