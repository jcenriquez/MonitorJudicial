using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MonitorJudicial
{
    public partial class ConsultaPorCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    // Llama al método para llenar el GridView solo si no es una solicitud de postback
            //    //LlenarGridView();
            //}

            if (!IsPostBack)
            {
                divTramitePrestamo.Visible = false;
            }
        }

        protected void LlenarGridViewCliente(string numeroCliente)
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
                WHERE p.CODIGOESTADOPRESTAMO='J' AND cli.NUMEROCLIENTE='" + numeroCliente + @"'
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

        protected void LlenarGridViewCedula(string numeroCedula)
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (rbCedula.Checked)  // Verificar si el radio button 'rbCedula' está seleccionado
            {
                LlenarGridViewCedula(idConsulta.Value);  // Ejecutar el método solo si 'rbCedula' está seleccionado
                divTramitePrestamo.Visible = true;
            }
            else
            {
                LlenarGridViewCliente(idConsulta.Value);
            }
        }


    }
}