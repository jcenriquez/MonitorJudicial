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
using ClosedXML.Excel;
using OfficeOpenXml;
using MonitorJudicial.Context;

namespace MonitorJudicial
{
    public partial class _Default : Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            string rol = (string)(Session["Rol"]);
            if (!IsPostBack)
            {
                //PorcentajeCasos();
                LlenarGridAbogados();
                //LlenarGridAbogadosPorcentajes();
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
            Response.Redirect("Views/Login.aspx");
        }


        public void LlenarGridAbogados()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    // Consulta utilizando LINQ para obtener los datos
                    var datos = context.EstadoPrestamos
                        .OrderByDescending(e => e.TOTAL) // Ordenar por TOTAL en orden descendente
                        .ToList();

                    // Cálculo de totales
                    int totalCastigado = datos.Sum(e => e.CASTIGADO);
                    int totalJudicial = datos.Sum(e => e.JUDICIAL);
                    int totalAlDia = datos.Sum(e => e.JUDICIAL_CON_ACUERDO_AL_DIA);
                    int totalVencido = datos.Sum(e => e.JUDICIAL_CON_ACUERDO_VENCIDO);
                    int totalPrejudicial = datos.Sum(e => e.PREJUDICIAL);
                    int sumaTotal = totalCastigado + totalJudicial + totalAlDia + totalPrejudicial + totalVencido;

                    // Asignar datos al GridView
                    gvCasosAbogado.DataSource = datos;
                    gvCasosAbogado.DataBind();

                    // Actualizar los literales con los totales
                    litPrestamoJudicial.Text = sumaTotal.ToString();
                    litotalCastigado.Text = totalCastigado.ToString();
                    litotalJudicial.Text = totalJudicial.ToString();
                    litotalAlDia.Text = totalAlDia.ToString();
                    litotalPrejudicial.Text = totalPrejudicial.ToString();
                    litotalVencido.Text = totalVencido.ToString();
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                //litError.Text = "Ocurrió un error al cargar los datos. Por favor, intente nuevamente.";
                // Registrar el error para depuración
                Console.WriteLine(ex.Message);
            }
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
                workSheet.Cells[2, 2].Value = "PREJUDICIAL";
                workSheet.Cells[2, 3].Value = "JUDICIAL";
                workSheet.Cells[2, 4].Value = "JUDICIAL CON ACUERDO AL DIA";
                workSheet.Cells[2, 5].Value = "JUDICIAL CON ACUERDO VENCIDO";
                workSheet.Cells[2, 6].Value = "CASTIGADOS";

                // Añadir datos
                workSheet.Cells[3, 1].Value = totalCasos;
                workSheet.Cells[3, 2].Value = prejudicial;
                workSheet.Cells[3, 3].Value = judicial;
                workSheet.Cells[3, 4].Value = alDia;
                workSheet.Cells[3, 5].Value = vencidos;
                workSheet.Cells[3, 6].Value = castigados;

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

        //protected void btnGenerarReporteExtendido_Click(object sender, EventArgs e)
        //{
        //    // Cadena de conexión a la base de datos
        //    string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

        //    // Crear un DataTable para almacenar los datos de la consulta
        //    DataTable dt = new DataTable();

        //    // Ejecutar el procedimiento almacenado
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("EXEC [FBS_REPORTES].[CONSULTARPRESTAMOS];", con))
        //        {
        //            con.Open();
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //        }
        //    }

        //    // Crear el archivo Excel en memoria usando ClosedXML
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        // Agregar el DataTable a una hoja de Excel
        //        wb.Worksheets.Add(dt, "Prestamos");

        //        // Configurar la respuesta para descargar el archivo
        //        HttpResponse response = HttpContext.Current.Response;
        //        response.Clear();
        //        response.Buffer = true;
        //        response.Charset = "";
        //        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        response.AddHeader("content-disposition", "attachment;filename=Prestamos.xlsx");

        //        // Guardar el archivo en la respuesta
        //        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //        {
        //            wb.SaveAs(memoryStream);
        //            memoryStream.WriteTo(response.OutputStream);
        //            response.Flush();
        //            response.End();
        //        }
        //    }
        //}

        protected void btnGenerarReporteExtendido_Click(object sender, EventArgs e)
        {
            // Crear el contexto de EF
            using (var context = new AppDbContext())
            {
                // Obtener los datos desde el procedimiento almacenado
                var prestamos = context.GetPrestamos().ToList();

                // Crear un DataTable para convertir los resultados
                var dt = new System.Data.DataTable();
                dt.Columns.Add("AGENCIA");
                dt.Columns.Add("NOMBRE_SOCIO");
                dt.Columns.Add("IDENTIDAD");
                dt.Columns.Add("NUMEROPRESTAMO");
                dt.Columns.Add("NUM_JUICIO");
                dt.Columns.Add("FECHA_ADJUDICACION");
                dt.Columns.Add("FECHA_INICIO_DEMANDA");
                dt.Columns.Add("DEUDAINICIAL", typeof(decimal));
                dt.Columns.Add("SALDO_ACTUAL", typeof(decimal));
                dt.Columns.Add("SALDO_TRANSFERIDO", typeof(decimal));
                dt.Columns.Add("NUM_CLIENTE");
                dt.Columns.Add("NOMBRE_ABOGADO");
                dt.Columns.Add("ESTADO_JUDICIAL");
                dt.Columns.Add("GARANTIA");
                dt.Columns.Add("JUZGADO");
                dt.Columns.Add("TRAMITE_JUDICIAL");

                foreach (var prestamo in prestamos)
                {
                    dt.Rows.Add(
                        prestamo.AGENCIA,
                        prestamo.NOMBRE_SOCIO,
                        prestamo.IDENTIDAD,
                        prestamo.NUMEROPRESTAMO,
                        prestamo.NUM_JUICIO,
                        prestamo.FECHA_ADJUDICACION,
                        prestamo.FECHA_INICIO_DEMANDA,
                        prestamo.DEUDAINICIAL,
                        prestamo.SALDO_ACTUAL,
                        prestamo.SALDO_TRANSFERIDO,
                        prestamo.NUM_CLIENTE,
                        prestamo.NOMBRE_ABOGADO,
                        prestamo.ESTADO_JUDICIAL,
                        prestamo.GARANTIA,
                        prestamo.JUZGADO,
                        prestamo.TRAMITE_JUDICIAL
                    );
                }

                // Crear el archivo Excel
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Prestamos");

                    // Configurar la respuesta para descargar el archivo
                    HttpResponse response = HttpContext.Current.Response;
                    response.Clear();
                    response.Buffer = true;
                    response.Charset = "";
                    response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    response.AddHeader("content-disposition", "attachment;filename=Prestamos.xlsx");

                    // Guardar el archivo en la respuesta
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.WriteTo(response.OutputStream);
                        response.Flush();
                        response.End();
                    }
                }
            }
        }

        protected void btnGenerarReporteDetalle_Click(object sender, EventArgs e)
        {
            // Cadena de conexión a la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            // Consulta SQL
            string query = @"SELECT  
                            PM.NUMEROPRESTAMO AS NUMERO_PRESTAMO,
                            CONCAT(UA.NOMBRES, ' ', UA.APELLIDOS) AS NOMBRE_ABOGADO,
                            PD.COMENTARIO,
                            CONVERT(VARCHAR(10), PD.FECHASISTEMA, 23) AS FECHA_ACTUALIZACION 
                         FROM 
                            [FBS_LOGS].[PRESTAMODEMANDAJUDICIALTRAMITE] PD
                            JOIN [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PD.SECUENCIALPRESTAMO = PM.SECUENCIAL
                            JOIN [FBS_SEGURIDADES].[USUARIO_ABOGADOS] UA ON UA.CODIGOABOGADO = PD.CODIGOABOGADO
                         ORDER BY 
                            PD.FECHASISTEMA DESC;";

            // Crear un DataTable para almacenar los datos de la consulta
            DataTable dt = new DataTable();

            // Conectar a la base de datos y ejecutar la consulta
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            // Crear el archivo Excel en memoria usando ClosedXML
            using (XLWorkbook wb = new XLWorkbook())
            {
                // Agregar el DataTable a una hoja de Excel
                wb.Worksheets.Add(dt, "Reporte");

                // Configurar la respuesta para descargar el archivo
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Buffer = true;
                response.Charset = "";
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("content-disposition", "attachment;filename=ReporteCasosActualizados.xlsx");

                // Guardar el archivo en la respuesta
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(response.OutputStream);
                    response.Flush();
                    response.End();
                }
            }
        }

        protected void btnGenerateReportDetalle_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query;
            string codigoAbogado = (string)(Session["CodigoAbogado"]);

            if (!string.IsNullOrEmpty(codigoAbogado))
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
                    CLI.NUMEROCLIENTE [NUM CLIENTE],
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
                    CLI.NUMEROCLIENTE [NUM CLIENTE],
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
                    [NOMBRE SOCIO];";

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
                                Response.AddHeader("content-disposition", "attachment;filename=ReporteDetallado.xlsx");

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
            else
            {
                // Si la sesión ha expirado, muestra el popup y redirige.
                string script = @"
                <script type='text/javascript'>
                    alert('La sesión ha expirado. Será redirigido a la página de inicio de sesión.');
                    window.location.href = 'Views/Login.aspx';
                </script>";

                // Registramos el script para ejecutarse en el cliente.
                ClientScript.RegisterStartupScript(this.GetType(), "SessionExpired", script);
            }
        }

    }
}