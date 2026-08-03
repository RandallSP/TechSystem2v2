using System;
using System.Data;
using System.Data.SqlClient;

namespace TechSystem2
{

public class TecnicoDatos
{
    public string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;

    public DataTable ListarTodos()
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT TecnicoID, Nombre, Especialidad FROM Tecnicos ORDER BY Nombre";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable Buscar(string texto)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT TecnicoID, Nombre, Especialidad FROM Tecnicos WHERE Nombre LIKE '%" + texto + "%' OR Especialidad LIKE '%" + texto + "%' ORDER BY Nombre";
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public DataTable ObtenerPorId(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        string sql = "SELECT TecnicoID, Nombre, Especialidad FROM Tecnicos WHERE TecnicoID = " + id;
        SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        con.Close();
        return dt;
    }

    public void Insertar(string nombre, string especialidad)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "INSERT INTO Tecnicos (Nombre, Especialidad) VALUES ('" + nombre + "', '" + especialidad + "')";
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void Actualizar(int id, string nombre, string especialidad)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "UPDATE Tecnicos SET Nombre = '" + nombre + "', Especialidad = '" + especialidad + "' WHERE TecnicoID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }

    public void Eliminar(int id)
    {
        SqlConnection con = new SqlConnection(conexion);
        con.Open();
        string sql = "DELETE FROM Tecnicos WHERE TecnicoID = " + id;
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        con.Close();
    }
}

}
