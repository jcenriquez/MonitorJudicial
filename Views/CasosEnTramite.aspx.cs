using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MonitorJudicial
{
    public partial class CasosEnTramite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LlenarGridViewCasos();
            }

        }
        public void LlenarGridViewCasos()
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                SELECT  
                    P.NUMEROPRESTAMO AS [NUMERO PRESTAMO], PER.NOMBREUNIDO AS [NOMBRE COMPLETO],P.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], P.DEUDAINICIAL AS [DEUDA INICIAL], 
                    P.SALDOACTUAL AS [SALDO ACTUAL], CONVERT(VARCHAR, P.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], CONVERT(VARCHAR, P.FECHAVENCIMIENTO, 23) AS [FECHA VENCIMIENTO],
                    ISNULL(ET.NOMBRE, '') AS [ESTADO], ISNULL(PT.COMENTARIO, '') AS [COMENTARIO], ISNULL(CONVERT(VARCHAR, PT.FECHASISTEMA, 23), '') AS [FECHA TRÁMITE] 
                FROM 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] P
                JOIN [FBS_PERSONAS].[PERSONA] PER ON P.IDENTIFICACIONSUJETOORIGINAL=PER.IDENTIFICACION
                JOIN [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = P.SECUENCIAL
                LEFT JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                WHERE P.CODIGOESTADOPRESTAMO='J'
                ORDER BY P.FECHAVENCIMIENTO DESC;";

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Asignar datos a la GridView
                gvCasosJudicial.DataSource = dataTable;
                gvCasosJudicial.DataBind();
            }
        }

        public DataTable LlenarDataTableCasos()
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
        SELECT  
            P.NUMEROPRESTAMO AS [NUMERO PRESTAMO], PER.NOMBREUNIDO AS [NOMBRE COMPLETO],P.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], P.DEUDAINICIAL AS [DEUDA INICIAL], 
            P.SALDOACTUAL AS [SALDO ACTUAL], CONVERT(VARCHAR, P.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], CONVERT(VARCHAR, P.FECHAVENCIMIENTO, 23) AS [FECHA VENCIMIENTO],
            ISNULL(ET.NOMBRE, '') AS [ESTADO], ISNULL(PT.COMENTARIO, '') AS [COMENTARIO], ISNULL(CONVERT(VARCHAR, PT.FECHASISTEMA, 23), '') AS [FECHA TRÁMITE] 
        FROM 
            [FBS_CARTERA].[PRESTAMOMAESTRO] P
        JOIN [FBS_PERSONAS].[PERSONA] PER ON P.IDENTIFICACIONSUJETOORIGINAL=PER.IDENTIFICACION
        JOIN [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
        LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = P.SECUENCIAL
        LEFT JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
        WHERE P.CODIGOESTADOPRESTAMO='J'
        ORDER BY P.FECHAVENCIMIENTO DESC;";

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Devolver la DataTable con los datos cargados
                return dataTable;
            }
        }

        protected void gvCasosJudicial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                // Establece el estilo de la fuente para cada celda en la fila de datos y encabezados
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Style["font-size"] = "0.65rem";
                    cell.Style["padding"] = "0.25rem";
                }
            }
        }

    }
}