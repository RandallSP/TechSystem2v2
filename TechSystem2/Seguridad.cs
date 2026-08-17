using System;
using System.Security.Cryptography;
using System.Text;

namespace TechSystem2
{

// Clase para convertir la clave en un codigo seguro (hash)
// De esta forma no guardamos la clave escrita en la base de datos
public class Seguridad
{
    // Convierte un texto en un hash de 64 caracteres
    // SHA256 es un metodo de encriptacion que no se puede deshacer
    public string ObtenerHash(string texto)
    {
        using (SHA256 sha = SHA256.Create())
        {
            // pasamos el texto a bytes y lo encriptamos
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(texto));

            // convertimos los bytes a un texto con letras y numeros
            StringBuilder resultado = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                resultado.Append(bytes[i].ToString("x2"));
            }
            return resultado.ToString();
        }
    }
}

}
