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
using System.Web.Configuration;
using OfficeOpenXml;
using System.IO;
using ClosedXML.Excel;

namespace MonitorJudicial
{
    public partial class CasosEnTramite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LlenarGridViewCasos();

            if (!IsPostBack)
            {
                CargarEstados();
                CargarEstadosJudiciales();
            }

        }
        protected void btnQuitarFiltro_Click(object sender, EventArgs e)
        {
            divGridPrincipal.Visible = true;
            divGridFiltrado.Visible = false;
            LlenarGridViewCasos();
        }
        protected void btnQuitarFiltroEstado_Click(object sender, EventArgs e)
        {
            divGridPrincipal.Visible = true;
            divGridFiltrado.Visible = false;
            LlenarGridViewCasos();
        }
        protected void CargarEstados()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string queryEstadoTramite = "SELECT NOMBRE FROM [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";

            ddlAccion.Items.Clear(); // Limpiar elementos previos

            // Agregar opción por defecto
            ddlAccion.Items.Add(new ListItem("...SELECCIONAR POR TRÁMITE...", ""));
            ddlAccion.Items.Add(new ListItem("...TODAS...", ""));

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryEstadoTramite, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ddlAccion.Items.Add(new ListItem(reader["NOMBRE"].ToString()));
                    }
                }
            }
        }
        protected void CargarEstadosJudiciales()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string queryEstadoTramite = "SELECT DISTINCT\r\n    CASE \r\n        WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'\r\n        WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'\r\n        WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'JUDICIAL CON ACUERDO AL DIA'\r\n        WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'\r\n        WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'JUDICIAL CON ACUERDO VENCIDO'\r\n    END AS [NOMBRE]\r\nFROM [FBS_CARTERA].[PRESTAMOMAESTRO] PM\r\nWHERE PM.CODIGOESTADOPRESTAMO IS NOT NULL\r\nAND PM.CODIGOESTADOPRESTAMO NOT IN ('T','Z','M')\r\nORDER BY NOMBRE;";

            ddlEstado.Items.Clear(); // Limpiar elementos previos

            // Agregar opción por defecto
            ddlEstado.Items.Add(new ListItem("...SELECCIONAR POR ESTADO JUDICIAL...", ""));
            ddlEstado.Items.Add(new ListItem("...TODAS...", ""));

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(queryEstadoTramite, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ddlEstado.Items.Add(new ListItem(reader["NOMBRE"].ToString()));
                    }
                }
            }
        }
        public void LlenarGridViewCasos()
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            string query;
            string codigoAbogado = (string)(Session["CodigoAbogado"]);

            if (codigoAbogado.Equals("0"))
            {
                // Consulta SQL
                query = @"
                    (
            SELECT 
                PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                PM.NUMEROPRESTAMO, 
                CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                PM.DEUDAINICIAL, 
                PM.SALDOACTUAL,
                AB.NOMBRE AS [NOMBRE ABOGADO],
                CASE 
                    WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                END AS [ESTADO JUDICIAL],
                ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
            FROM 
                [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
            INNER JOIN 
                [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
            JOIN 
                [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
            JOIN 
                [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
            LEFT JOIN 
                [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
            WHERE 
                PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
        )
        UNION ALL
        (
            SELECT 
                PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                PM.NUMEROPRESTAMO, 
                CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                PM.DEUDAINICIAL, 
                PM.SALDOACTUAL,
                AB.NOMBRE AS [NOMBRE ABOGADO],
                CASE 
                    WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                END AS [ESTADO JUDICIAL],
                ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
            FROM 
                [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
            INNER JOIN 
                [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
            INNER JOIN 
                [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
            JOIN 
                [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
            JOIN 
                [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
            LEFT JOIN 
                [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
            WHERE 
                PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
        )
        ORDER BY 
            [FECHA ADJUDICACION] DESC;";

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
            else
            {
                // Consulta SQL
                query = @"
                    (
            SELECT 
                PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                PM.NUMEROPRESTAMO, 
                CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                PM.DEUDAINICIAL, 
                PM.SALDOACTUAL,
                AB.NOMBRE AS [NOMBRE ABOGADO],
                CASE 
                    WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                END AS [ESTADO JUDICIAL],
                ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
            FROM 
                [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
            INNER JOIN 
                [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
            JOIN 
                [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
            JOIN 
                [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
            LEFT JOIN 
                [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
            WHERE 
                PA.CODIGOABOGADO = '" + codigoAbogado + @"'
                AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
        )
        UNION ALL
        (
            SELECT 
                PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                PM.NUMEROPRESTAMO, 
                CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                PM.DEUDAINICIAL, 
                PM.SALDOACTUAL,
                AB.NOMBRE AS [NOMBRE ABOGADO],
                CASE 
                    WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                END AS [ESTADO JUDICIAL],
                ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
            FROM 
                [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
            INNER JOIN 
                [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
            INNER JOIN 
                [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
            JOIN 
                [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
            JOIN 
                [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
            LEFT JOIN 
                [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
            LEFT JOIN 
                [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
            WHERE 
                PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                AND PBJ.CODIGOABOGADO = '" + codigoAbogado + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
        )
        ORDER BY 
            [FECHA ADJUDICACION] DESC;";

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
        }
        static string filtro = "";
        public void LlenarGridViewCasosFiltrado(string filtroTramiteJudicial)
        {
            filtro = filtroTramiteJudicial;
            divGridPrincipal.Visible = false;
            divGridFiltrado.Visible = true;
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query;
            string codigoAbogado = (string)(Session["CodigoAbogado"]);
            if (filtroTramiteJudicial == "")
            {
                divGridPrincipal.Visible = true;
                divGridFiltrado.Visible = false;
                LlenarGridViewCasos();
            }
            else
            {
                // Cadena de conexión a la base de datos


                if (codigoAbogado.Equals("0"))
                {
                    query = @"
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                            AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                            AND ET.NOMBRE= '" + filtroTramiteJudicial + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    UNION ALL
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                        INNER JOIN 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                            AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                            AND ET.NOMBRE= '" + filtroTramiteJudicial + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    ORDER BY 
                        [FECHA ADJUDICACION] DESC;";

                    // Establecer conexión y ejecutar la consulta
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        // Asignar datos a la GridView
                        gvCasosJudicialFiltrado.DataSource = dataTable;
                        gvCasosJudicialFiltrado.DataBind();
                    }
                }
                else
                {
                    query = @"
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PA.CODIGOABOGADO  = '" + codigoAbogado + @"'
                            AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                            AND ET.NOMBRE= '" + filtroTramiteJudicial + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    UNION ALL
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                        INNER JOIN 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                            AND PBJ.CODIGOABOGADO = '" + codigoAbogado + @"'
                            AND ET.NOMBRE= '" + filtroTramiteJudicial + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    ORDER BY 
                        [FECHA ADJUDICACION] DESC;";




                    //    // Consulta SQL
                    //    string query = @"
                    //                    (
                    //    SELECT 
                    //        PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    //        PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    //        PM.NUMEROPRESTAMO, 
                    //        CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    //        PM.DEUDAINICIAL, 
                    //        PM.SALDOACTUAL,
                    //        AB.NOMBRE AS [NOMBRE ABOGADO],
                    //        CASE 
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
                    //        END AS [ESTADO JUDICIAL],
                    //        ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                    //    FROM 
                    //        [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                    //    INNER JOIN 
                    //        [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                    //    JOIN 
                    //        [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                    //    JOIN 
                    //        [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                    //    WHERE 
                    //        PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                    //        AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                    //  AND ET.NOMBRE= '" + filtroTramiteJudicial + @"'
                    //)
                    //UNION ALL
                    //(
                    //    SELECT 
                    //        PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    //        PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    //        PM.NUMEROPRESTAMO, 
                    //        CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    //        PM.DEUDAINICIAL, 
                    //        PM.SALDOACTUAL,
                    //        AB.NOMBRE AS [NOMBRE ABOGADO],
                    //        CASE 
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
                    //        END AS [ESTADO JUDICIAL],
                    //        ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                    //    FROM 
                    //        [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                    //    INNER JOIN 
                    //        [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    INNER JOIN 
                    //        [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                    //    JOIN 
                    //        [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                    //    JOIN 
                    //        [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                    //    WHERE 
                    //        PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                    //        AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                    //  AND ET.NOMBRE= '" + filtroTramiteJudicial + @"'
                    //)
                    //ORDER BY 
                    //    [FECHA ADJUDICACION] DESC;";

                    // Establecer conexión y ejecutar la consulta
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        // Asignar datos a la GridView
                        gvCasosJudicialFiltrado.DataSource = dataTable;
                        gvCasosJudicialFiltrado.DataBind();
                    }
                }

            }

        }

        public void LlenarGridViewCasosFiltradoEstado(string filtroTramiteJudicial)
        {
            string codigoPrestamo = "";

            switch (filtroTramiteJudicial)
            {
                case "CASTIGADO":
                    codigoPrestamo = "G";
                    break;
                case "JUDICIAL":
                    codigoPrestamo = "J";
                    break;
                case "JUDICIAL CON ACUERDO AL DIA":
                    codigoPrestamo = "A";
                    break;
                case "PREJUDICIAL":
                    codigoPrestamo = "I";
                    break;
                case "JUDICIAL CON ACUERDO VENCIDO":
                    codigoPrestamo = "V";
                    break;
                default:
                    codigoPrestamo = ""; 

                    break;
            }





            filtro = filtroTramiteJudicial;
            divGridPrincipal.Visible = false;
            divGridFiltrado.Visible = true;
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query;
            string codigoAbogado = (string)(Session["CodigoAbogado"]);
            if (filtroTramiteJudicial == "")
            {
                divGridPrincipal.Visible = true;
                divGridFiltrado.Visible = false;
                LlenarGridViewCasos();
            }
            else
            {
                if (codigoAbogado.Equals("0"))
                {
                    query = @"
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                            AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                            AND PM.CODIGOESTADOPRESTAMO= '" + codigoPrestamo + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    UNION ALL
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                        INNER JOIN 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                            AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                            AND PM.CODIGOESTADOPRESTAMO= '" + codigoPrestamo + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    ORDER BY 
                        [FECHA ADJUDICACION] DESC;";

                    // Establecer conexión y ejecutar la consulta
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        // Asignar datos a la GridView
                        gvCasosJudicialFiltrado.DataSource = dataTable;
                        gvCasosJudicialFiltrado.DataBind();
                    }
                }
                else
                {
                    query = @"
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PA.CODIGOABOGADO  = '" + codigoAbogado + @"'
                            AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                            AND PM.CODIGOESTADOPRESTAMO= '" + codigoPrestamo + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    UNION ALL
                    (
                        SELECT 
                            PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                            PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                            PM.NUMEROPRESTAMO, 
                            CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                            PM.DEUDAINICIAL, 
                            PM.SALDOACTUAL,
                            AB.NOMBRE AS [NOMBRE ABOGADO],
                            CASE 
                                WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                                WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                            END AS [ESTADO JUDICIAL],
                            ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                        FROM 
                            [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                        INNER JOIN 
                            [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        INNER JOIN 
                            [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                        JOIN 
                            [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                        JOIN 
                            [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                        LEFT JOIN 
                            [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                        LEFT JOIN 
                            [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                        WHERE 
                            PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                            AND PBJ.CODIGOABOGADO = '" + codigoAbogado + @"'
                            AND PM.CODIGOESTADOPRESTAMO= '" + codigoPrestamo + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    )
                    ORDER BY 
                        [FECHA ADJUDICACION] DESC;";




                    //    // Consulta SQL
                    //    string query = @"
                    //                    (
                    //    SELECT 
                    //        PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    //        PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    //        PM.NUMEROPRESTAMO, 
                    //        CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    //        PM.DEUDAINICIAL, 
                    //        PM.SALDOACTUAL,
                    //        AB.NOMBRE AS [NOMBRE ABOGADO],
                    //        CASE 
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
                    //        END AS [ESTADO JUDICIAL],
                    //        ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                    //    FROM 
                    //        [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                    //    INNER JOIN 
                    //        [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                    //    JOIN 
                    //        [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                    //    JOIN 
                    //        [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                    //    WHERE 
                    //        PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                    //        AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')
                    //  AND ET.NOMBRE= '" + filtroTramiteJudicial + @"'
                    //)
                    //UNION ALL
                    //(
                    //    SELECT 
                    //        PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    //        PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    //        PM.NUMEROPRESTAMO, 
                    //        CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    //        PM.DEUDAINICIAL, 
                    //        PM.SALDOACTUAL,
                    //        AB.NOMBRE AS [NOMBRE ABOGADO],
                    //        CASE 
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                    //            WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
                    //        END AS [ESTADO JUDICIAL],
                    //        ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL]
                    //    FROM 
                    //        [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                    //    INNER JOIN 
                    //        [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    INNER JOIN 
                    //        [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                    //    JOIN 
                    //        [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                    //    JOIN 
                    //        [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    //    LEFT JOIN 
                    //        [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                    //    WHERE 
                    //        PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                    //        AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                    //  AND ET.NOMBRE= '" + filtroTramiteJudicial + @"'
                    //)
                    //ORDER BY 
                    //    [FECHA ADJUDICACION] DESC;";

                    // Establecer conexión y ejecutar la consulta
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        adapter.Fill(dataTable);

                        // Asignar datos a la GridView
                        gvCasosJudicialFiltrado.DataSource = dataTable;
                        gvCasosJudicialFiltrado.DataBind();
                    }
                }

            }

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpiar las variables de sesión
            Session["Rol"] = null;
            Session["Nombres"] = null;
            Session["CodigoAbogado"] = null;
            Session["NumPretamo"] = null;
            Session["EmailAbogado"] = null;
            Session["CodigoUsuario"] = null;

            // O puedes usar Session.Clear() para limpiar todas las variables de sesión
            // Session.Clear();

            // Redirigir a la página de inicio de sesión
            Response.Redirect("Login.aspx");
        }
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if ((filtro == "...SELECCIONAR POR TRÁMITE...") || (filtro == "...TODAS..."))
            {
                divGridPrincipal.Visible = true;
                divGridFiltrado.Visible = false;
                LlenarGridViewCasos();
            }
            else
            {
                LlenarGridViewCasosFiltrado(ddlAccion.SelectedValue);
            }
        }
        protected void btnFiltrarEstado_Click(object sender, EventArgs e)
        {
            if ((filtro == "...SELECCIONAR POR ESTADO JUDICIAL...") || (filtro == "...TODAS..."))
            {
                divGridPrincipal.Visible = true;
                divGridFiltrado.Visible = false;
                LlenarGridViewCasos();
            }
            else
            {
                LlenarGridViewCasosFiltradoEstado(ddlEstado.SelectedValue);
            }
        }

        protected void gvCasosJudicial_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCasosJudicial.PageIndex = e.NewPageIndex;
            LlenarGridViewCasos();
        }
        protected void gvCasosJudicialFiltrado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCasosJudicialFiltrado.PageIndex = e.NewPageIndex;
            LlenarGridViewCasosFiltrado(ddlAccion.SelectedValue);
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
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query;
            string codigoAbogado = (string)(Session["CodigoAbogado"]);

            if (codigoAbogado.Equals("0"))
            {
                query = @"
                            (
                SELECT 
                    PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    PM.NUMEROPRESTAMO, 
                    pai.NUMEROCAUSA AS [N CAUSA],
                    CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    PM.DEUDAINICIAL, 
                    PM.SALDOACTUAL,
                    AB.NOMBRE AS [NOMBRE ABOGADO],
                    CASE 
                        WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                    END AS [ESTADO JUDICIAL],
                    ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL],
                    PT.FECHAREMATE AS [FECHA REMATE],
                    PT.COMENTARIO AS [COMENTARIO],
                    DIV.NOMBRE AS [OFICINA],
                    MC.NOMBRE AS [MEDIDA CAUTELAR],
                    ISNULL(DATEDIFF(DAY, PT.FECHASISTEMA, GETDATE()), '') AS [DIAS DESDE TRAMITE JUDICIAL]
                FROM 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                INNER JOIN 
                    [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                JOIN 
                    [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                JOIN 
                    [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                JOIN 
                    [FBS_GENERALES].[DIVISION] DIV ON DIV.SECUENCIAL = PM.SECUENCIALOFICINA
                JOIN 
                    [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] MC ON MC.CODIGO = PA.CODIGOTIPOMEDCAUTELAR
                JOIN 
                    [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai ON PA.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
                WHERE 
                    PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
                    AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                )
                UNION ALL
                (
                SELECT 
                    PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    PM.NUMEROPRESTAMO, 
                    '' AS [N CAUSA],
                    CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    PM.DEUDAINICIAL, 
                    PM.SALDOACTUAL,
                    AB.NOMBRE AS [NOMBRE ABOGADO],
                    CASE 
                        WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                    END AS [ESTADO JUDICIAL],
                    ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL],
                    PT.FECHAREMATE AS [FECHA REMATE],
                    PT.COMENTARIO AS [COMENTARIO],
                    DIV.NOMBRE AS [OFICINA],
                    '' AS [MEDIDA CAUTELAR],
                    ISNULL(DATEDIFF(DAY, PT.FECHASISTEMA, GETDATE()), '')  AS [DIAS DESDE TRAMITE JUDICIAL]
                FROM 
                    [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                INNER JOIN 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                INNER JOIN 
                    [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                JOIN 
                    [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                JOIN 
                    [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                JOIN 
                    [FBS_GENERALES].[DIVISION] DIV ON DIV.SECUENCIAL = PM.SECUENCIALOFICINA
                WHERE 
                    PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                    AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                )
                ORDER BY 
                    [FECHA ADJUDICACION] DESC;";
            }
            else
            {
                query = @"
                            (
                SELECT 
                    PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    PM.NUMEROPRESTAMO, 
                    pai.NUMEROCAUSA AS [N CAUSA],
                    CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    PM.DEUDAINICIAL, 
                    PM.SALDOACTUAL,
                    AB.NOMBRE AS [NOMBRE ABOGADO],
                    CASE 
                        WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                    END AS [ESTADO JUDICIAL],
                    ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL],
                    PT.FECHAREMATE AS [FECHA REMATE],
PT.COMENTARIO AS [COMENTARIO],
                    DIV.NOMBRE AS [OFICINA],
                    MC.NOMBRE AS [MEDIDA CAUTELAR],
                    ISNULL(DATEDIFF(DAY, PT.FECHASISTEMA, GETDATE()), '') AS [DIAS DESDE TRAMITE JUDICIAL]
                FROM 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
                INNER JOIN 
                    [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
                JOIN 
                    [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                JOIN 
                    [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                JOIN 
                    [FBS_GENERALES].[DIVISION] DIV ON DIV.SECUENCIAL = PM.SECUENCIALOFICINA
                JOIN 
                    [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] MC ON MC.CODIGO = PA.CODIGOTIPOMEDCAUTELAR
                JOIN 
                    [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai ON PA.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
                WHERE 
                    PA.CODIGOABOGADO = '" + codigoAbogado + @"'
                    AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J') AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                )
                UNION ALL
                (
                SELECT 
                    PER.NOMBREUNIDO AS [NOMBRE SOCIO], 
                    PM.IDENTIFICACIONSUJETOORIGINAL AS [IDENTIDAD], 
                    PM.NUMEROPRESTAMO, 
                    '' AS [N CAUSA],
                    CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [FECHA ADJUDICACION], 
                    PM.DEUDAINICIAL, 
                    PM.SALDOACTUAL,
                    AB.NOMBRE AS [NOMBRE ABOGADO],
                    CASE 
                        WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                        WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
WHEN PM.CODIGOESTADOPRESTAMO = 'M' THEN 'MOROSO'
                    END AS [ESTADO JUDICIAL],
                    ISNULL(ET.NOMBRE, '') AS [TRAMITE JUDICIAL],
                    PT.FECHAREMATE AS [FECHA REMATE],
PT.COMENTARIO AS [COMENTARIO],
                    DIV.NOMBRE AS [OFICINA],
                    '' AS [MEDIDA CAUTELAR],
                    ISNULL(DATEDIFF(DAY, PT.FECHASISTEMA, GETDATE()), '')  AS [DIAS DESDE TRAMITE JUDICIAL]
                FROM 
                    [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
                INNER JOIN 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
                INNER JOIN 
                    [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
                JOIN 
                    [FBS_PERSONAS].[PERSONA] PER ON PM.IDENTIFICACIONSUJETOORIGINAL = PER.IDENTIFICACION
                JOIN 
                    [FBS_CLIENTES].[CLIENTE] CLI ON PER.[SECUENCIAL] = CLI.[SECUENCIALPERSONA]
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PM.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                JOIN 
                    [FBS_GENERALES].[DIVISION] DIV ON DIV.SECUENCIAL = PM.SECUENCIALOFICINA
                WHERE 
                    PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
                    AND PBJ.CODIGOABOGADO = '" + codigoAbogado + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                )
                ORDER BY 
                    [FECHA ADJUDICACION] DESC;";
            }




            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Reporte");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=Reporte.xlsx");

                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }
        }


    }
}