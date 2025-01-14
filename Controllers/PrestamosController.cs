using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Net.Mail;
using System.Net;


namespace MonitorJudicial.Controllers
{
    public class PrestamosController
    {
        private static string buzonFinancialI = "info@coopsanantonio.com";
        private static string buzonAdministradorI = "rvillarreal@coopsanantonio.com";
        private static string servidorI = "mail.coopsanantonio.com";
        private static int puertoCorreoI = 25;

        private static string buzonFinancialE = "info@coopsanantonio.com";
        private static string buzonAdministradorE = "coacsanantonio@hotmail.es";
        private static string servidorE = "mail.coopsanantonio.com";
        private static int puertoCorreoE = 25;
        private static string uidFinancialE = "info@coopsanantonio.com";
        private static string clvFinancialE = "coac1960";

        public static void EnviarCorreo(string destinatario, string asunto, string cuerpo, string copiaCC = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(buzonFinancialI);
                mail.To.Add(destinatario);

                if (!string.IsNullOrEmpty(copiaCC))
                {
                    mail.CC.Add(copiaCC);
                }

                mail.Subject = asunto;
                mail.Body = cuerpo;

                SmtpClient smtpClient = new SmtpClient(servidorI, puertoCorreoI);
                smtpClient.Credentials = new NetworkCredential(uidFinancialE, clvFinancialE);
                smtpClient.EnableSsl = false; // No SSL as per the settings provided

                smtpClient.Send(mail);
                Console.WriteLine("Correo enviado exitosamente a " + destinatario);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }        
    }
}