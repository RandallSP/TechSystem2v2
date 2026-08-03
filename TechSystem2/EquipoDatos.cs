using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

public class EquipoDatos
{
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    public DataTable ListarTodos()
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT e.EquipoID, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario FROM Equipos e LEFT JOIN Usuarios u ON e.UsuarioID = u.UsuarioID ORDER BY e.TipoEquipo";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable Buscar(string texto)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT e.EquipoID, e.TipoEquipo, e.Modelo, u.Nombre AS NombreUsuario FROM Equipos e LEFT JOIN Usuarios u ON e.UsuarioID = u.UsuarioID WHERE e.TipoEquipo LIKE '%" + texto + "%' OR e.Modelo LIKE '%" + texto + "%' OR u.Nombre LIKE '%" + texto + "%' ORDER BY e.TipoEquipo";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable ObtenerPorId(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT EquipoID, TipoEquipo, Modelo, UsuarioID FROM Equipos WHERE EquipoID = " + id;
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable ListarUsuarios()
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT UsuarioID, Nombre FROM Usuarios ORDER BY Nombre";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public void Insertar(string tipo, string modelo, int idUsuario)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "INSERT INTO Equipos (TipoEquipo, Modelo, UsuarioID) VALUES ('" + tipo + "', '" + modelo + "', " + idUsuario + ")";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void Actualizar(int id, string tipo, string modelo, int idUsuario)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "UPDATE Equipos SET TipoEquipo = '" + tipo + "', Modelo = '" + modelo + "', UsuarioID = " + idUsuario + " WHERE EquipoID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void Eliminar(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "DELETE FROM Equipos WHERE EquipoID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }
}

}
