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


namespace MonitorJudicial.Controllers
{
    public class PrestamosController
    {        
        public static void LlenarGridViewCedula(string numeroCedula, GridView gvPrestamos, HtmlInputText txtNombres)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
                tp.NOMBRE AS [TIPO],
                p.DEUDAINICIAL AS [DEUDA INICIAL],
                p.SALDOACTUAL AS [SALDO],
                CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
                CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
                e.NOMBRE AS [ESTADO]
                FROM [FBS_CARTERA].[PRESTAMOMAESTRO] p 
                JOIN [FBS_CREDITO].[TIPOPRESTAMO] tp ON p.CODIGOTIPOPRESTAMO = tp.CODIGO
                JOIN [FBS_CARTERA].[ESTADOPRESTAMO] e ON p.CODIGOESTADOPRESTAMO=e.CODIGO
                JOIN [FBS_PERSONAS].[PERSONA] per ON p.IDENTIFICACIONSUJETOORIGINAL=per.IDENTIFICACION
                JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
                WHERE p.CODIGOESTADOPRESTAMO='J' AND PER.IDENTIFICACION='" + numeroCedula + @"'
                ORDER BY p.SECUENCIAL DESC";

            string queryNombre = @"
                SELECT per.IDENTIFICACION AS [CEDULA],
                    cli.NUMEROCLIENTE AS [CLIENTE],
                    per.NOMBREUNIDO AS [NOMBRES]
                FROM [FBS_PERSONAS].[PERSONA] per
                JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
                WHERE per.IDENTIFICACION='" + numeroCedula + @"'";

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

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryNombre, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreApellido = reader["NOMBRES"].ToString();
                        txtNombres.Value = nombreApellido;
                    }

                    reader.Close();
                }
            }
        }
        public static void LlenarGridViewCliente(string numeroCliente, GridView gvPrestamos, HtmlInputText txtNombres)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                SELECT TOP (1000) p.NUMEROPRESTAMO AS [N° PRÉSTAMO],
                tp.NOMBRE AS [TIPO],
                p.DEUDAINICIAL AS [DEUDA INICIAL],
                p.SALDOACTUAL AS [SALDO],
                CONVERT(VARCHAR, p.FECHAADJUDICACION, 23) AS [ADJUDICADO],
                CONVERT(VARCHAR, p.FECHAVENCIMIENTO, 23) AS [VENCIMIENTO],
                e.NOMBRE AS [ESTADO]
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

            string queryNombre = @"
                SELECT per.IDENTIFICACION AS [CEDULA],
                    cli.NUMEROCLIENTE AS [CLIENTE],
                    per.NOMBREUNIDO AS [NOMBRES]
                FROM [FBS_PERSONAS].[PERSONA] per
                JOIN [FBS_CLIENTES].[CLIENTE] cli ON per.[SECUENCIAL] = cli.[SECUENCIALPERSONA]
                WHERE cli.NUMEROCLIENTE='" + numeroCliente + @"'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryNombre, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreApellido = reader["NOMBRES"].ToString();
                        txtNombres.Value = nombreApellido;
                    }

                    reader.Close();
                }
            }
        }
    }
}