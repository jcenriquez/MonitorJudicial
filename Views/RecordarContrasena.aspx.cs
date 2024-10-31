using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MonitorJudicial.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MonitorJudicial.Views
{
    public partial class RecordarContrasena : System.Web.UI.Page
    {
        private static readonly string EncryptionKey = "l8hYQ6IY5FkFzRL9u7XvhTjzGBkeVwBjx+X/zXNL8Do=";
        private static string codigoUsuario = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSendResetLink_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string clave = "";

            // Validar que el correo exista en la base de datos
            if (EmailExistsInDatabase(email))
            {
                clave = TraerClave(email);
                // Generar token único para restablecer la contraseña
                //string resetToken = Guid.NewGuid().ToString();
                //SaveResetTokenToDatabase(email, resetToken);

                //// Enviar el correo con el enlace de restablecimiento
                //string resetLink = $"https://www.tusitio.com/ResetPassword.aspx?token={resetToken}";
                SendResetLinkEmail(email, clave, codigoUsuario);

                lblMessage.Text = "La contraseña ha sido enviada a su correo electrónico.";
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "El correo ingresado no está registrado.";
            }
        }

        private bool EmailExistsInDatabase(string email)
        {
            // Obtener la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Definir el comando SQL para buscar el correo electrónico
                string query = "SELECT [CODIGOUSUARIO], [CLAVE], [EMAIL] FROM [FBS_SEGURIDADES].[USUARIO_ABOGADOS] WHERE [EMAIL] = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetro de correo electrónico para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        // Abrir la conexión a la base de datos
                        connection.Open();

                        // Ejecutar la consulta y verificar si devuelve filas
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            respuesta = reader.HasRows; // Si hay filas, el correo existe
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, se podría registrar o manejar según la lógica
                        Console.WriteLine("Error al verificar el correo en la base de datos: " + ex.Message);
                    }
                }
            }

            return respuesta;
        }

        private string TraerClave(string email)
        {
            // Obtener la cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";
            //string codigoUsuario;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Definir el comando SQL para buscar el correo electrónico
                string query = "SELECT [CODIGOUSUARIO], [CLAVE], [EMAIL] FROM [FBS_SEGURIDADES].[USUARIO_ABOGADOS] WHERE [EMAIL] = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Agregar parámetro de correo electrónico para evitar inyecciones SQL
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        // Abrir la conexión a la base de datos
                        connection.Open();

                        // Ejecutar la consulta y verificar si devuelve filas
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string encryptedPassword = reader["CLAVE"].ToString();
                                codigoUsuario = reader["CODIGOUSUARIO"].ToString();
                                if (!string.IsNullOrEmpty(encryptedPassword))
                                {
                                    respuesta = DecryptString(encryptedPassword);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, se podría registrar o manejar según la lógica
                        Console.WriteLine("Error al verificar el correo en la base de datos: " + ex.Message);
                    }
                }
            }

            return respuesta;
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

        //private void SaveResetTokenToDatabase(string email, string resetToken)
        //{
        //    // Guarda el token en la base de datos asociado al usuario
        //    // Implementación ficticia - reemplaza con una consulta real
        //}

        private void SendResetLinkEmail(string email, string clave, string usuario)
        {
            //string usuario = txtUsuario.Text.ToUpper();
            //string nombres = txtFirstName.Text.ToUpper();
            //string apellidos = txtLastName.Text.ToUpper();
            //string email = txtEmail.Text;
            string copiaEmail = "jenriquez@coopsanantonio.com"; //"fpuedmag@coopsanantonio.com";
            //int rol = ddlCategories.SelectedIndex;
            //string nombreAbogado = ddlAbogado.SelectedItem.ToString();
            //string codigoAbogado = CargarCodigoAbogado(nombreAbogado);
            //string password = txtPassword.Text;
            //string repeatPassword = txtRepeatPassword.Text;
            string asunto = "Usuario y contraseña - Monitor Judicial";
            string cuerpo = $@"Estimado/a,

Para ingresar al sitio web Monitor Judicial, por favor realizarlo con la siguiente información:

Usuario: {usuario}
Contraseña: {clave}

Puede acceder al sitio utilizando o copiando en tu navegador el siguiente enlace:
http://192.100.100.58:8088/

Mensaje Generado Desde Monitor Judicial 1.3.

La información contenida en este e-mail es confidencial y solo puede ser utilizada por la persona o la institución a la cual está dirigido. Cualquier retención, difusión, distribución o copia de este mensaje está prohibida. La institución no asume responsabilidad sobre información, opiniones o criterios contenidos en este mail que no estén relacionados con negocios oficiales de nuestra institución. Si usted recibió este mensaje por error, notifique al administrador o a quien le envió inmediatamente, elimínelo sin ver su contenido o hacer copias. (Las tildes se han omitido para facilitar la lectura).";


            PrestamosController.EnviarCorreo(email, asunto, cuerpo, copiaEmail);




            //MailMessage mail = new MailMessage();
            //mail.To.Add(email);
            //mail.From = new MailAddress("tuemail@dominio.com");
            //mail.Subject = "Restablecimiento de contraseña";
            //mail.Body = $"Haga clic en el siguiente enlace para restablecer su contraseña: {clave}";
            //mail.IsBodyHtml = true;

            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "smtp.dominio.com";
            //smtp.Port = 587; // Cambia según tu servidor
            //smtp.Credentials = new System.Net.NetworkCredential("usuario", "contraseña");
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
        }
    }
}