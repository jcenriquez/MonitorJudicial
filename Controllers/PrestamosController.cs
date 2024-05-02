using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;


namespace MonitorJudicial.Controllers
{
    public class PrestamosController
    {
        public static void LlenarGridViewCedula(string numeroCedula, GridView gvPrestamos)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° Préstamo],
                tp.NOMBRE AS [Tipo],
                p.DEUDAINICIAL AS [Deuda Inicial],
                p.SALDOACTUAL AS [Saldo],
                CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [Adjudicado],
                CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [Vencimiento],
                e.NOMBRE AS [Estado]
                FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
                JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
                JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
                JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
                JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
                WHERE p.CODIGOESTADOPRESTAMO='J' AND PER.IDENTIFICACION='" + numeroCedula + @"'
                ORDER BY p.SECUENCIAL DESC";

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Asignar datos a la GridView
                gvPrestamos.DataSource = dataTable;
                gvPrestamos.DataBind();
            }
        }
        public static void LlenarGridViewCliente(string numeroCliente, GridView gvPrestamos)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° Préstamo],
                tp.NOMBRE AS [Tipo],
                p.DEUDAINICIAL AS [Deuda Inicial],
                p.SALDOACTUAL AS [Saldo],
                CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [Adjudicado],
                CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [Vencimiento],
                e.NOMBRE AS [Estado]
                FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
                JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
                JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
                JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
                JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
                WHERE p.CODIGOESTADOPRESTAMO='J'  AND cli.NUMEROCLIENTE='" + numeroCliente + @"'
                ORDER BY p.SECUENCIAL DESC";

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Asignar datos a la GridView
                gvPrestamos.DataSource = dataTable;
                gvPrestamos.DataBind();
            }
        }
    }
}