﻿using System;
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
        //public void LlenarGridViewCedula(string numeroCedula, GridView gvPrestamos, HtmlInputText txtNombres)
        //{
        //    // Cadena de conexión a la base de datos
        //    string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        //    string query;
        //    string codigoAbogado = (string)(Session["CodigoAbogado"]);

        //    if (codigoAbogado.Equals("0"))
        //    {
        //        query = @"
        //        SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //        tp.NOMBRE AS [TIPO],
        //        p.DEUDAINICIAL AS [DEUDA INICIAL],
        //        p.SALDOACTUAL AS [SALDO],
        //        CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //        CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //        e.NOMBRE AS [ESTADO]
        //        FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //        JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //        JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //        JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //        JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //        WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G') AND PER.IDENTIFICACION='" + numeroCedula + @"'
        //        ORDER BY p.SECUENCIAL DESC";
        //    }
        //    else
        //    {
        //        query = @"
        //        SELECT p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //        tp.NOMBRE AS [TIPO],
        //        p.DEUDAINICIAL AS [DEUDA INICIAL],
        //        p.SALDOACTUAL AS [SALDO],
        //        CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //        CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //        e.NOMBRE AS [ESTADO]
        //        FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //        JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //        JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //        JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //        JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //     left JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PA.SECUENCIALPRESTAMO = p.SECUENCIAL
        //        left JOIN [FBS_COBRANZAS].[ABOGADO] AB ON AB.CODIGO = PA.CODIGOABOGADO
        //        WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G') AND PER.IDENTIFICACION='" + numeroCedula + @"'
        //     AND AB.CODIGO= '" + codigoAbogado + @"'
        //        ORDER BY p.SECUENCIAL DESC";
        //    }

        //    string queryNombre = @"
        //        SELECT per.IDENTIFICACION AS [CEDULA],
        //            cli.NUMEROCLIENTE AS [CLIENTE],
        //            per.NOMBREUNIDO AS [NOMBRES]
        //        FROM [FBS_PERSONAS].[PERSONA] per
        //        JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //        WHERE per.IDENTIFICACION='" + numeroCedula + @"'";

        //    // Establecer conexión y ejecutar la consulta
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        DataTable dataTable = new DataTable();

        //        adapter.Fill(dataTable);

        //        // Asignar datos a la GridView
        //        gvPrestamos.DataSource = dataTable;
        //        gvPrestamos.DataBind();
        //    }

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(queryNombre, connection))
        //        {
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                string nombreApellido = reader["NOMBRES"].ToString();
        //                txtNombres.Value = nombreApellido;
        //            }

        //            reader.Close();
        //        }
        //    }
        //}

        //        public static void LlenarGridViewCaso(string numeroCedula, GridView gvPrestamos, HtmlInputText txtNombres)
        //        {
        //            // Cadena de conexión a la base de datos
        //            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

        //            string query;
        //            string codigoAbogado = (string)(Session["CodigoAbogado"]);

        //            if (codigoAbogado.Equals("0"))
        //            {
        //                query = @"
        //        SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //tp.NOMBRE AS [TIPO],
        //p.DEUDAINICIAL AS [DEUDA INICIAL],
        //p.SALDOACTUAL AS [SALDO],
        //CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //e.NOMBRE AS [ESTADO]
        //FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] pa ON pa.SECUENCIALPRESTAMO=p.SECUENCIAL
        //JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai ON pa.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
        //WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G')
        //AND pai.NUMEROCAUSA='" + numeroCedula + @"'
        //ORDER BY p.SECUENCIAL DESC;";
        //            }
        //            else
        //            {
        //                query = @"
        //        SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //tp.NOMBRE AS [TIPO],
        //p.DEUDAINICIAL AS [DEUDA INICIAL],
        //p.SALDOACTUAL AS [SALDO],
        //CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //e.NOMBRE AS [ESTADO]
        //FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] pa ON pa.SECUENCIALPRESTAMO=p.SECUENCIAL
        //JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai ON pa.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
        //left JOIN [FBS_COBRANZAS].[ABOGADO] AB ON AB.CODIGO = pa.CODIGOABOGADO
        //WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G')
        //AND pai.NUMEROCAUSA='" + numeroCedula + @"'
        //AND AB.CODIGO= '" + codigoAbogado + @"'
        //ORDER BY p.SECUENCIAL DESC;";
        //            }

        //            string queryNombre = @"
        //        SELECT per.IDENTIFICACION AS [CEDULA],
        //        cli.NUMEROCLIENTE AS [CLIENTE],
        //        per.NOMBREUNIDO AS [NOMBRES]
        //    FROM [FBS_PERSONAS].[PERSONA] per
        //    JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //	JOIN [FBS_CARTERA].[PRESTAMOMAESTRO] p ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //	JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] pa ON pa.SECUENCIALPRESTAMO=p.SECUENCIAL
        //JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai ON pa.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
        //    WHERE pai.NUMEROCAUSA='" + numeroCedula + @"';";

        //            // Establecer conexión y ejecutar la consulta
        //            using (SqlConnection connection = new SqlConnection(connectionString))
        //            {
        //                SqlCommand command = new SqlCommand(query, connection);
        //                SqlDataAdapter adapter = new SqlDataAdapter(command);
        //                DataTable dataTable = new DataTable();

        //                adapter.Fill(dataTable);

        //                // Asignar datos a la GridView
        //                gvPrestamos.DataSource = dataTable;
        //                gvPrestamos.DataBind();
        //            }

        //            using (SqlConnection connection = new SqlConnection(connectionString))
        //            {
        //                using (SqlCommand command = new SqlCommand(queryNombre, connection))
        //                {
        //                    connection.Open();
        //                    SqlDataReader reader = command.ExecuteReader();

        //                    while (reader.Read())
        //                    {
        //                        string nombreApellido = reader["NOMBRES"].ToString();
        //                        txtNombres.Value = nombreApellido;
        //                    }

        //                    reader.Close();
        //                }
        //            }
        //        }

        //    public static void LlenarGridViewCliente(string numeroCliente, GridView gvPrestamos, HtmlInputText txtNombres)
        //    {
        //        // Cadena de conexión a la base de datos
        //        string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

        //        // Consulta SQL
        //        string query;
        //        string codigoAbogado = (string)(Session["CodigoAbogado"]);

        //        if (codigoAbogado.Equals("0"))
        //        {
        //            query = @"
        //            SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //            tp.NOMBRE AS [TIPO],
        //            p.DEUDAINICIAL AS [DEUDA INICIAL],
        //            p.SALDOACTUAL AS [SALDO],
        //            CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //            CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //            e.NOMBRE AS [ESTADO]
        //            FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //            JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //            JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //            JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //            JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //            WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G')  AND cli.NUMEROCLIENTE='" + numeroCliente + @"'
        //            ORDER BY p.SECUENCIAL DESC";
        //        }
        //        else
        //        {
        //            query = @"
        //            SELECT  p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
        //            tp.NOMBRE AS [TIPO],
        //            p.DEUDAINICIAL AS [DEUDA INICIAL],
        //            p.SALDOACTUAL AS [SALDO],
        //            CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
        //            CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
        //            e.NOMBRE AS [ESTADO]
        //            FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
        //            JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
        //            JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
        //            JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
        //            JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //left JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PA.SECUENCIALPRESTAMO = p.SECUENCIAL
        //left JOIN [FBS_COBRANZAS].[ABOGADO] AB ON AB.CODIGO = PA.CODIGOABOGADO
        //            WHERE p.CODIGOESTADOPRESTAMO IN ('J','I','G')  AND cli.NUMEROCLIENTE='" + numeroCliente + @"'
        //AND AB.CODIGO= '" + codigoAbogado + @"'
        //            ORDER BY p.SECUENCIAL DESC";
        //        }

        //        // Establecer conexión y ejecutar la consulta
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand command = new SqlCommand(query, connection);
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            DataTable dataTable = new DataTable();

        //            adapter.Fill(dataTable);

        //            // Asignar datos a la GridView
        //            gvPrestamos.DataSource = dataTable;
        //            gvPrestamos.DataBind();
        //        }

        //        string queryNombre = @"
        //            SELECT per.IDENTIFICACION AS [CEDULA],
        //                cli.NUMEROCLIENTE AS [CLIENTE],
        //                per.NOMBREUNIDO AS [NOMBRES]
        //            FROM [FBS_PERSONAS].[PERSONA] per
        //            JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
        //            WHERE cli.NUMEROCLIENTE='" + numeroCliente + @"'";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            using (SqlCommand command = new SqlCommand(queryNombre, connection))
        //            {
        //                connection.Open();
        //                SqlDataReader reader = command.ExecuteReader();

        //                while (reader.Read())
        //                {
        //                    string nombreApellido = reader["NOMBRES"].ToString();
        //                    txtNombres.Value = nombreApellido;
        //                }

        //                reader.Close();
        //            }
        //        }
        //    }
    }
}