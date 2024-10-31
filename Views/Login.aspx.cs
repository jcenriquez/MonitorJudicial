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
            if (!IsPostBack)
            {
                // Cargar cookies si existen
                if (Request.Cookies["Usuario"] != null)
                {
                    txtUsuario.Text = Request.Cookies["Usuario"].Value;
                }
                if (Request.Cookies["Password"] != null)
                {
                    txtPassword.Attributes["value"] = Request.Cookies["Password"].Value;
                }
                if (Request.Cookies["Recordarme"] != null)
                {
                    chkRecordarme.Checked = Request.Cookies["Recordarme"].Value == "true";
                }
            }
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
            //string usuario = txtUsuario.Text.ToUpper();
            //string password = txtPassword.Text;


            //string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            //if (ValidarUsuario(usuario, password, connectionString))
            //{
            //    string role = (string)(Session["Rol"]);
            //    if (role.Equals("1"))
            //    {
            //        Response.Redirect("/Default.aspx");
            //    }
            //    else
            //    {
            //        Response.Redirect("~/Views/ConsultaPorCliente.aspx");
            //    }

            //}
            //else
            //{
            //    Response.Write("<script>alert('Usuario o contraseña incorrectos');</script>");
            //}

            string usuario = txtUsuario.Text.ToUpper();
            string password = txtPassword.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Llama al método para validar usuario
            if (ValidarUsuario(usuario, password, connectionString))
            {
                // Almacena el rol en la sesión
                string role = (string)(Session["Rol"]);

                // Si el checkbox de "Recuérdame" está activado, almacena las cookies
                if (chkRecordarme.Checked)
                {
                    HttpCookie usuarioCookie = new HttpCookie("Usuario", usuario);
                    HttpCookie passwordCookie = new HttpCookie("Password", password);
                    HttpCookie recordarCookie = new HttpCookie("Recordarme", "true");

                    // Configura la duración de las cookies (ej. 30 días)
                    usuarioCookie.Expires = DateTime.Now.AddDays(30);
                    passwordCookie.Expires = DateTime.Now.AddDays(30);
                    recordarCookie.Expires = DateTime.Now.AddDays(30);

                    // Agrega las cookies a la respuesta
                    Response.Cookies.Add(usuarioCookie);
                    Response.Cookies.Add(passwordCookie);
                    Response.Cookies.Add(recordarCookie);
                }
                else
                {
                    // Borra las cookies si el checkbox no está seleccionado
                    if (Request.Cookies["Usuario"] != null)
                    {
                        Response.Cookies["Usuario"].Expires = DateTime.Now.AddDays(-1);
                    }
                    if (Request.Cookies["Password"] != null)
                    {
                        Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                    }
                    if (Request.Cookies["Recordarme"] != null)
                    {
                        Response.Cookies["Recordarme"].Expires = DateTime.Now.AddDays(-1);
                    }
                }

                // Redirección según el rol
                if (role.Equals("1"))
                {
                    Response.Redirect("/Default.aspx");
                }
                else
                {
                    Response.Redirect("~/Views/ConsultaPorCliente.aspx");
                }
            }
            else
            {
                // Mensaje de error si la autenticación falla
                Response.Write("<script>alert('Usuario o contraseña incorrectos');</script>");
            }
        }


        private bool ValidarUsuario(string usuario, string password, string connectionString)
        {
            bool isValid = false;
            string roles;
            string nombres;
            string codigoAbogado;
            string correoAbogado;
            string codigoUsuario;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"SELECT [CODIGOUSUARIO], [CLAVE], [EMAIL], [NOMBRES], [APELLIDOS], [ROL], 
                                    [FECHA_CREACION], [ESTADO_ACTIVO], [CODIGOABOGADO] 
                             FROM [FBS_SEGURIDADES].[USUARIO_ABOGADOS] 
                             WHERE [CODIGOUSUARIO] = @usuario";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@usuario", usuario);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string encryptedPassword = reader["CLAVE"].ToString();
                            if (!string.IsNullOrEmpty(encryptedPassword))
                            {
                                string decryptedPassword = DecryptString(encryptedPassword);
                                string temp = decryptedPassword;
                                string temp2 = "";
                                if (decryptedPassword == password)
                                {
                                    isValid = true;
                                    roles = reader["ROL"].ToString(); // Asigna el valor de la columna ROL a la variable roles
                                    Session["Rol"] = roles;
                                    nombres = reader["NOMBRES"].ToString() +" "+ reader["APELLIDOS"].ToString();
                                    Session["Nombres"] = nombres;
                                    codigoAbogado = reader["CODIGOABOGADO"].ToString(); // Asigna el valor de la columna ROL a la variable roles
                                    Session["CodigoAbogado"] = codigoAbogado;
                                    correoAbogado = reader["EMAIL"].ToString(); // Asigna el valor de la columna ROL a la variable roles
                                    Session["EmailAbogado"] = correoAbogado;
                                    codigoUsuario = reader["CODIGOUSUARIO"].ToString();
                                    Session["CodigoUsuario"] = codigoUsuario;
                                }
                            }
                        }
                    }
                }
            }

            return isValid;
        }

    }
}