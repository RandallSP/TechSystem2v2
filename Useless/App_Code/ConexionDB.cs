using System; // por si acaso
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Clase que maneja la conexion a la base de datos TechSystemDB.
/// Capa de acceso a datos - aqui se abre y cierra la conexion.
/// </summary>
public class ConexionDB
{
    // variable privada que guarda la conexion
    private SqlConnection conexion;

    /// <summary>
    /// Constructor: crea la conexion usando el connection string del Web.config
    /// </summary>
    public ConexionDB()
    {
        string cadenaConexion = ConfigurationManager.ConnectionStrings["ConexionDB"].ConnectionString;
        conexion = new SqlConnection(cadenaConexion);
    }

    /// <summary>
    /// Abre la conexion a la base de datos.
    /// Si ya esta abierta, no hace nada.
    /// </summary>
    public void Abrir()
    {
        try
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al abrir la conexion con la base de datos: " + ex.Message);
        }
    }

    /// <summary>
    /// Cierra la conexion a la base de datos.
    /// Si ya esta cerrada, no hace nada.
    /// </summary>
    public void Cerrar()
    {
        try
        {
            if (conexion.State != ConnectionState.Closed)
            {
                conexion.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al cerrar la conexion con la base de datos: " + ex.Message);
        }
    }

    /// <summary>
    /// Devuelve el objeto SqlConnection para usarlo con SqlCommand, SqlDataAdapter, etc.
    /// </summary>
    public SqlConnection ObtenerConexion()
    {
        return conexion;
    }
}
