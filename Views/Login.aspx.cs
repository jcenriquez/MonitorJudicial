using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MonitorJudicial.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static string GenerateAESKey(int keySize)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.GenerateKey();
                return Convert.ToBase64String(aes.Key);
            }
        }

        private static readonly string EncryptionKey = "l8hYQ6IY5FkFzRL9u7XvhTjzGBkeVwBjx+X/zXNL8Do="; // Clave generada

        public static string EncryptString(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Convert.FromBase64String(EncryptionKey);

            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("La clave especificada no tiene un tamaño válido para este algoritmo.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // Vector de inicialización de 16 bytes

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                        cs.Close();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Convert.FromBase64String(EncryptionKey);

            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("La clave especificada no tiene un tamaño válido para este algoritmo.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16]; // Vector de inicialización de 16 bytes

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string password = txtPassword.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            if (ValidarUsuario(usuario, password, connectionString))
            {
                Response.Redirect("/Default.aspx");
            }
            else
            {
                Response.Write("<script>alert('Usuario o contraseña incorrectos');</script>");
            }
        }

        private bool ValidarUsuario(string usuario, string password, string connectionString)
        {
            bool isValid = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT [CLAVE] FROM [FBS_Respaldo_DC_Produccion].[FBS_SEGURIDADES].[USUARIO_ABOGADOS] WHERE [CODIGOUSUARIO] = @usuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", usuario);

                    connection.Open();
                    string encryptedPassword = (string)command.ExecuteScalar();

                    if (!string.IsNullOrEmpty(encryptedPassword))
                    {
                        string decryptedPassword = DecryptString(encryptedPassword);

                        if (decryptedPassword == password)
                        {
                            isValid = true;
                        }
                    }
                }
            }

            return isValid;
        }


    }
}