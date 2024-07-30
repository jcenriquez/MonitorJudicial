using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MonitorJudicial.Views
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string usuario = txtusuario.Text;
            string nombres = txtFirstName.Text;
            string apellidos = txtLastName.Text;
            string email = txtEmail.Text;
            int rol = ddlCategories.SelectedIndex;
            string codigoAbogado = "0";
            string password = txtPassword.Text;
            string repeatPassword = txtRepeatPassword.Text;

            InsertarNuevoUsuario(usuario, password, email, nombres, apellidos, rol, codigoAbogado);
            // Implementar la lógica de validación y almacenamiento de datos aquí
            Response.Write("<script>alert('Usuario creado con éxito');</script>");
            // Redireccionar a la página de inicio de sesión, por ejemplo
            Response.Redirect("login.aspx");
        }

        public void InsertarNuevoUsuario(string usuario, string password, string email, string nombres, string apellidos, int rol, string codigoAbogado)
        { 
           
            string encryptedPassword = EncryptString(password);
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO [FBS_Respaldo_DC_Produccion].[FBS_SEGURIDADES].[USUARIO_ABOGADOS] " +
                               "([CODIGOUSUARIO], [CLAVE], [EMAIL], [NOMBRES], [APELLIDOS], [ROL], [FECHA_CREACION], [ESTADO_ACTIVO], [CODIGOABOGADO]) " +
                               "VALUES (@usuario, @password, @correo, @nombres, @apellidos, @rol, @fechaCreacion, @estadoActivo, @codigoAbogado)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", usuario);
                    command.Parameters.AddWithValue("@password", encryptedPassword);
                    command.Parameters.AddWithValue("@correo", email);
                    command.Parameters.AddWithValue("@nombres", nombres);
                    command.Parameters.AddWithValue("@apellidos", apellidos);
                    command.Parameters.AddWithValue("@rol", rol);
                    command.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);
                    command.Parameters.AddWithValue("@estadoActivo", true); // O el valor que consideres adecuado
                    command.Parameters.AddWithValue("@codigoAbogado", codigoAbogado);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
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
    }
}