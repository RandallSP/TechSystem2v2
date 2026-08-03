using LOGIN.CapaDatos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;

namespace LOGIN.CapaLogica
{
    public class usuario
    {
        public void Agregarusuario() { }
        public void Consultarusuario() { }
        public void Modificarusuario() { }
        public void Borrarusuario() { }


        public static int  validausuario(string correo, string clave)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;

            using (SqlConnection conexion = new SqlConnection(connectionString))
            using (SqlCommand comando = new SqlCommand("SELECT email, clave, nombre  FROM usuario WHERE email = @correo AND clave = @clave", conexion))
            {
                // Usar parámetros evita inyección SQL
                comando.Parameters.AddWithValue("@correo", correo);
                comando.Parameters.AddWithValue("@clave", clave);
                conexion.Open();
                using (SqlDataReader registro = comando.ExecuteReader())
                {
                    if (registro.Read())
                    {
                        cls_Usuario.nombre = registro["nombre"].ToString();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }


    }
}