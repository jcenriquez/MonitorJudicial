using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace MonitorJudicial
{
    public partial class _Default : Page
    {
        protected double abandono { get; set; }
        protected double abstencionTrmiteParteJuez { get; set; }
        protected double adjudicacion { get; set; }
        protected double alegatos { get; set; }
        protected double apelacion { get; set; }
        protected double avaluoBienes { get; set; }
        protected double calificacionDemanda { get; set; }
        protected double cambioCasilleroJudicial { get; set; }
        protected double citacionDemandados { get; set; }
        protected double contestacionDemanda { get; set; }
        protected double desistimiento { get; set; }
        protected double embargo { get; set; }
        protected double juntaConciliacion { get; set; }
        protected double liquidacion { get; set; }
        protected double mandamientoEjecucion { get; set; }
        protected double ninguno { get; set; }
        protected double noContestaDemanda { get; set; }
        protected double otros { get; set; }
        protected double presentacionDemanda { get; set; }
        protected double prueba { get; set; }
        protected double remate { get; set; }
        protected double sentencia { get; set; }
        protected double suspendidoAcuerdo { get; set; }
        protected double terminadoAcuerdoPagoObligaciones { get; set; }
        protected double aprehencionVehicular { get; set; }
        protected double audiencia { get; set; }
        protected double razonNoPago { get; set; }
        protected double audienciaEjecucion { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PorcentajeCasos();
                LlenarGridAbogados();
            }
        }
        public void LlenarGridAbogados()
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"
                WITH CombinedData AS (
        SELECT AB.NOMBRE, 
               CASE 
                   WHEN PM.CODIGOESTADOPRESTAMO = 'G' THEN 'CASTIGADO'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'J' THEN 'JUDICIAL'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
               END AS Tipo
        FROM [FBS_CARTERA].[PRESTAMOMAESTRO] PM 
        LEFT JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PM.SECUENCIAL = PA.SECUENCIALPRESTAMO
        INNER JOIN [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO = AB.CODIGO
        WHERE PA.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
          AND PM.CODIGOESTADOPRESTAMO IN ('G', 'J')

        UNION ALL

        SELECT AB.NOMBRE, 
               CASE 
                   WHEN PM.CODIGOESTADOPRESTAMO = 'A' THEN 'AL DIA'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'I' THEN 'PREJUDICIAL'
                   WHEN PM.CODIGOESTADOPRESTAMO = 'V' THEN 'VENCIDO'
               END AS Tipo
        FROM [FBS_COBRANZAS].[PRESTAMOABOGADOPREJUDICIAL] PBJ
        INNER JOIN [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PBJ.SECUENCIALPRESTAMO = PM.SECUENCIAL
        INNER JOIN [FBS_COBRANZAS].[ABOGADO] AB ON PBJ.CODIGOABOGADO = AB.CODIGO
        WHERE PM.CODIGOESTADOPRESTAMO IN ('A', 'I', 'V')
          AND PBJ.CODIGOABOGADO IN ('1003372438', '1001715265', '1002739819', '1001623519', '1001669405')
    )
    SELECT ISNULL(NOMBRE, 'TOTAL') AS NOMBRE,
           ISNULL([CASTIGADO], 0) AS CASTIGADO,
           ISNULL([JUDICIAL], 0) AS JUDICIAL,
           ISNULL([AL DIA], 0) AS [AL DIA],
           ISNULL([PREJUDICIAL], 0) AS PREJUDICIAL,
           ISNULL([VENCIDO], 0) AS VENCIDO,
           ISNULL([CASTIGADO], 0) + ISNULL([JUDICIAL], 0) + ISNULL([AL DIA], 0) + ISNULL([PREJUDICIAL], 0) + ISNULL([VENCIDO], 0) AS TOTAL
    FROM CombinedData
    PIVOT (
        COUNT(Tipo)
        FOR Tipo IN ([CASTIGADO], [JUDICIAL], [AL DIA], [PREJUDICIAL], [VENCIDO])
    ) AS PivotTable
    ORDER BY 
        CASE 
            WHEN NOMBRE IS NULL THEN 1 
            ELSE 0 
        END, 
        NOMBRE";

            int sumaTotal = 0;
            // Contadores para cada tipo de préstamo
            int totalCastigado = 0;
            int totalJudicial = 0;
            int totalAlDia = 0;
            int totalPrejudicial = 0;
            int totalVencido = 0;

            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                

                // Iterar sobre las filas del DataTable y contar los distintos tipos
                foreach (DataRow row in dataTable.Rows)
                {
                    totalCastigado += Convert.ToInt32(row["CASTIGADO"]);
                    totalJudicial += Convert.ToInt32(row["JUDICIAL"]);
                    totalAlDia += Convert.ToInt32(row["AL DIA"]);
                    totalPrejudicial += Convert.ToInt32(row["PREJUDICIAL"]);
                    totalVencido += Convert.ToInt32(row["VENCIDO"]);
                }

                // Asignar datos a la GridView
                gvCasosAbogado.DataSource = dataTable;
                gvCasosAbogado.DataBind();

                // Imprimir los totales en la consola (o puedes usar estos valores según tu necesidad)
                //Console.WriteLine($"Total Castigado: {totalCastigado}");
                //Console.WriteLine($"Total Judicial: {totalJudicial}");
                //Console.WriteLine($"Total Al Dia: {totalAlDia}");
                //Console.WriteLine($"Total Prejudicial: {totalPrejudicial}");
                //Console.WriteLine($"Total Vencido: {totalVencido}");
                sumaTotal = totalCastigado + totalJudicial + totalAlDia + totalPrejudicial + totalVencido;
            }

            litPrestamoJudicial.Text = sumaTotal.ToString();
            litotalCastigado.Text = totalCastigado.ToString();
            litotalJudicial.Text = totalJudicial.ToString();
            litotalAlDia.Text = totalAlDia.ToString();
            litotalPrejudicial.Text = totalPrejudicial.ToString();
            litotalVencido.Text = totalVencido.ToString();
        }

        protected void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            // Establecer el contexto de la licencia
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Obtiene los valores de los controles
            string totalCasos = litPrestamoJudicial.Text;
            string castigados = litotalCastigado.Text;
            string judicial = litotalJudicial.Text;
            string alDia = litotalAlDia.Text;
            string prejudicial = litotalPrejudicial.Text;
            string vencidos = litotalVencido.Text;

            using (ExcelPackage excel = new ExcelPackage())
            {
                var workSheet = excel.Workbook.Worksheets.Add("Reporte");

                // Añadir cabecera
                workSheet.Cells[1, 1, 1, 6].Merge = true;
                workSheet.Cells[1, 1].Value = "REPORTE EXCEL CASOS PRÉSTAMOS EN ESTADOS JUDICIALES";
                workSheet.Cells[1, 1].Style.Font.Size = 14;
                workSheet.Cells[1, 1].Style.Font.Bold = true;
                workSheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Añadir encabezados
                workSheet.Cells[2, 1].Value = "TOTAL CASOS";
                workSheet.Cells[2, 2].Value = "CASTIGADOS";
                workSheet.Cells[2, 3].Value = "JUDICIAL";
                workSheet.Cells[2, 4].Value = "AL DÍA";
                workSheet.Cells[2, 5].Value = "PREJUDICIAL";
                workSheet.Cells[2, 6].Value = "VENCIDOS";

                // Añadir datos
                workSheet.Cells[3, 1].Value = totalCasos;
                workSheet.Cells[3, 2].Value = castigados;
                workSheet.Cells[3, 3].Value = judicial;
                workSheet.Cells[3, 4].Value = alDia;
                workSheet.Cells[3, 5].Value = prejudicial;
                workSheet.Cells[3, 6].Value = vencidos;

                // Ajustar tamaño de columnas
                workSheet.Cells.AutoFitColumns();

                // Añadir datos del GridView
                if (gvCasosAbogado.Rows.Count > 0)
                {
                    int startRow = 5; // La fila donde comenzarán los datos del GridView
                    int colIndex = 1;

                    // Añadir encabezados del GridView
                    for (int i = 0; i < gvCasosAbogado.HeaderRow.Cells.Count; i++)
                    {
                        workSheet.Cells[startRow, colIndex + i].Value = gvCasosAbogado.HeaderRow.Cells[i].Text;
                    }

                    // Añadir datos del GridView
                    for (int i = 0; i < gvCasosAbogado.Rows.Count; i++)
                    {
                        for (int j = 0; j < gvCasosAbogado.Rows[i].Cells.Count; j++)
                        {
                            workSheet.Cells[startRow + i + 1, colIndex + j].Value = gvCasosAbogado.Rows[i].Cells[j].Text;
                        }
                    }

                    // Ajustar tamaño de columnas
                    workSheet.Cells[startRow, 1, startRow + gvCasosAbogado.Rows.Count, gvCasosAbogado.HeaderRow.Cells.Count].AutoFitColumns();
                }

                // Generar el archivo Excel
                string excelName = $"Reporte_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName);
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public void PorcentajeCasos()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            string query = @"
                SELECT 
                    ISNULL(ET.NOMBRE, 'NINGUNO') AS [ESTADO],
                    COUNT(*) AS [TOTAL_REGISTROS],
                    COUNT(*) * 100.0 / (SELECT COUNT(*) 
                                        FROM [FBS_CARTERA].[PRESTAMOMAESTRO] P
                                        LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = P.SECUENCIAL
                                        LEFT JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                                        WHERE P.CODIGOESTADOPRESTAMO IN ('J','I','G')) AS [PORCENTAJE]
                FROM 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] P
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = P.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                WHERE 
                    P.CODIGOESTADOPRESTAMO IN ('J','I','G')
                GROUP BY 
                    ISNULL(ET.NOMBRE, 'NINGUNO');";


            Dictionary<string, double> porcentajes = new Dictionary<string, double>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string estado = reader["ESTADO"].ToString();
                    //double porcentaje = Math.Round(Convert.ToDouble(reader["PORCENTAJE"]), 2);
                    double porcentaje = Math.Round(Convert.ToDouble(reader["TOTAL_REGISTROS"]), 2);

                    // Agregamos el porcentaje al diccionario utilizando el estado como clave
                    porcentajes.Add(estado, porcentaje);
                }

                reader.Close();
            }

            // Ahora, puedes acceder a cada porcentaje utilizando el nombre del estado como clave en el diccionario
            // Ahora, los porcentajes están almacenados en el arreglo 'porcentajes' en el mismo orden que los estados.
            // Puedes acceder a cada porcentaje por su índice en el arreglo.

            abandono = porcentajes.ContainsKey("ABANDONO") ? porcentajes["ABANDONO"] : 0;
            abstencionTrmiteParteJuez = porcentajes.ContainsKey("ABSTENCIÓN DE TRÁMITE POR PARTE DEL JUEZ") ? porcentajes["ABSTENCIÓN DE TRÁMITE POR PARTE DEL JUEZ"] : 0;
            adjudicacion = porcentajes.ContainsKey("ADJUDICACIÓN ") ? porcentajes["ADJUDICACIÓN "] : 0;
            alegatos = porcentajes.ContainsKey("ALEGATOS") ? porcentajes["ALEGATOS"] : 0;
            apelacion = porcentajes.ContainsKey("APELACIÓN ") ? porcentajes["APELACIÓN "] : 0;
            avaluoBienes = porcentajes.ContainsKey("AVALUÓ DE BIENES") ? (int)porcentajes["AVALUÓ DE BIENES"] : 0;
            calificacionDemanda = porcentajes.ContainsKey("CALIFICACIÓN DEMANDA") ? porcentajes["CALIFICACIÓN DEMANDA"] : 0;
            cambioCasilleroJudicial = porcentajes.ContainsKey("CAMBIO DE CASILLERO JUDICIAL") ? porcentajes["CAMBIO DE CASILLERO JUDICIAL"] : 0;
            citacionDemandados = porcentajes.ContainsKey("CITACIÓN A LOS DEMANDADOS ") ? porcentajes["CITACIÓN A LOS DEMANDADOS "] : 0;
            contestacionDemanda = porcentajes.ContainsKey("CONTESTACIÓN DEMANDA") ? porcentajes["CONTESTACIÓN DEMANDA"] : 0;
            desistimiento = porcentajes.ContainsKey("DESISTIMIENTO") ? porcentajes["DESISTIMIENTO"] : 0;
            embargo = porcentajes.ContainsKey("EMBARGO") ? porcentajes["EMBARGO"] : 0;
            juntaConciliacion = porcentajes.ContainsKey("JUNTA DE CONCILIACIÓN ") ? porcentajes["JUNTA DE CONCILIACIÓN "] : 0;
            liquidacion = porcentajes.ContainsKey("LIQUIDACIÓN ") ? porcentajes["LIQUIDACIÓN "] : 0;
            mandamientoEjecucion = porcentajes.ContainsKey("MANDAMIENTO DE EJECUCIÓN ") ? porcentajes["MANDAMIENTO DE EJECUCIÓN "] : 0;
            ninguno = porcentajes.ContainsKey("NINGUNO") ? porcentajes["NINGUNO"] : 0;
            noContestaDemanda = porcentajes.ContainsKey("NO CONTESTA DEMANDA") ? porcentajes["NO CONTESTA DEMANDA"] : 0;
            otros = porcentajes.ContainsKey("OTROS") ? porcentajes["OTROS"] : 0;
            presentacionDemanda = porcentajes.ContainsKey("PRESENTACIÓN DEMANDA") ? porcentajes["PRESENTACIÓN DEMANDA"] : 0;
            prueba = porcentajes.ContainsKey("PRUEBA") ? porcentajes["PRUEBA"] : 0;
            remate = porcentajes.ContainsKey("REMATE") ? porcentajes["REMATE"] : 0;
            sentencia = porcentajes.ContainsKey("SENTENCIA") ? porcentajes["SENTENCIA"] : 0;
            suspendidoAcuerdo = porcentajes.ContainsKey("SUSPENDIDO POR ACUERDO") ? porcentajes["SUSPENDIDO POR ACUERDO"] : 0;
            terminadoAcuerdoPagoObligaciones = porcentajes.ContainsKey("TERMINADO POR ACUERDO O PAGO DE OBLIGACIONES") ? porcentajes["TERMINADO POR ACUERDO O PAGO DE OBLIGACIONES"] : 0;
            aprehencionVehicular = porcentajes.ContainsKey("APREHENCION VEHICULAR") ? porcentajes["APREHENCION VEHICULAR"] : 0;
            audiencia = porcentajes.ContainsKey("AUDIENCIA") ? porcentajes["AUDIENCIA"] : 0;
            razonNoPago = porcentajes.ContainsKey("RAZÓN DE NO PAGO") ? porcentajes["RAZÓN DE NO PAGO"] : 0;
            audienciaEjecucion = porcentajes.ContainsKey("AUDIENCIA EJECUCIÓN") ? porcentajes["AUDIENCIA EJECUCIÓN"] : 0;

        }
    }
}