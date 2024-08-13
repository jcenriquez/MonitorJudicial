using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MonitorJudicial.Controllers;

namespace MonitorJudicial.Views
{
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarFormulario();

        }
        protected void CargarFormulario()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string queryAbogado = "SELECT NOMBRE FROM [FBS_COBRANZAS].[ABOGADO] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
           
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryAbogado, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreAbogado = reader["NOMBRE"].ToString();
                        ddlAbogado.Items.Add(new ListItem(nombreAbogado));
                    }

                    reader.Close();
                }
            }
            
        }

        protected string CargarCodigoAbogado(string nombreAbogado)
        {
            string respuesta = "";

            if(nombreAbogado.Equals("ADMINISTRATIVO COAC"))
            {
                respuesta = "0";
            }
            else
            {
                string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
                string queryAbogado = "SELECT CODIGO FROM [FBS_COBRANZAS].[ABOGADO] WHERE NOMBRE = @NombreAbogado ORDER BY NOMBRE";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(queryAbogado, connection))
                    {
                        command.Parameters.AddWithValue("@NombreAbogado", nombreAbogado);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            respuesta = reader["CODIGO"].ToString();
                        }

                        reader.Close();
                    }
                }
            }            
            return respuesta;
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.ToUpper();
            string nombres = txtFirstName.Text.ToUpper();
            string apellidos = txtLastName.Text.ToUpper();
            string email = txtEmail.Text;
            string copiaEmail= "fpuedmag@coopsanantonio.com";
            int rol = ddlCategories.SelectedIndex;
            string nombreAbogado = ddlAbogado.SelectedItem.ToString();
            string codigoAbogado = CargarCodigoAbogado(nombreAbogado);
            string password = txtPassword.Text;
            string repeatPassword = txtRepeatPassword.Text;
            string asunto = "Creación Usuario - Monitor Judicial";
            string cuerpo = $@"Estimado {nombres} {apellidos},

Se ha creado un usuario para ingresar al sitio web Monitor Judicial con la siguiente información:

Usuario: {usuario}
Contraseña: {password}
Abogado: {nombreAbogado}

Mensaje Generado Desde Monitor Judicial 1.0.

La información contenida en este e-mail es confidencial y solo puede ser utilizada por la persona o la institución a la cual está dirigido. Cualquier retención, difusión, distribución o copia de este mensaje está prohibida. La institución no asume responsabilidad sobre información, opiniones o criterios contenidos en este mail que no estén relacionados con negocios oficiales de nuestra institución. Si usted recibió este mensaje por error, notifique al administrador o a quien le envió inmediatamente, elimínelo sin ver su contenido o hacer copias. (Las tildes se han omitido para facilitar la lectura).";

            ValidatePage();
            try
            {
                // Implementar la lógica de validación y almacenamiento de datos aquí
                InsertarNuevoUsuario(usuario, password, email, nombres, apellidos, rol, codigoAbogado);
                PrestamosController.EnviarCorreo(email,asunto,cuerpo,copiaEmail);
                // Mostrar mensaje de éxito y redirigir a login después de mostrar el mensaje
                string script = "alert('Usuario creado con éxito'); window.location='Login.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "UsuarioCreado", script, true);
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error
                string script = "alert('Error!: No se pudo guardar el usuario');";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorGuardarUsuario", script, true);
            }
        }

        public void InsertarNuevoUsuario(string usuario, string password, string email, string nombres, string apellidos, int rol, string codigoAbogado)
        { 
           
            string encryptedPassword = EncryptString(password);
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO [FBS_SEGURIDADES].[USUARIO_ABOGADOS] " +
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

        private void ValidatePage()
        {
            // Ejecuta todas las validaciones del lado del servidor
            Page.Validate();

            // Verifica si todas las validaciones del lado del cliente y del servidor son válidas
            if (Page.IsValid)
            {
                // Todas las validaciones se cumplieron
                // Aquí puedes colocar el código que deseas ejecutar cuando todas las validaciones son exitosas
                Response.Write("Todas las validaciones se cumplieron correctamente.");
            }
            else
            {
                // Al menos una validación falló
                // Aquí puedes colocar el código que deseas ejecutar cuando alguna validación falla
                Response.Write("Al menos una validación falló. Por favor revise los errores.");
            }
        }
    }
}