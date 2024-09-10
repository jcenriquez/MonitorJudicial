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
        protected static string secuencialPrestamo;
        protected static string codigoAbogado;
        protected static string secuencial = "";
        protected static string codigoestadotramitedemjud = "";
        protected static string codigoabogado = "";
        protected static string comentario = "";
        protected static string estaactivo = "";
        protected static string numeroverificador = "";
        protected static string codigousuario;
        protected static string fechasistema = "";
        protected static string fechamaquina = "";
        protected static string medicaCautelar = "";
        protected static string judicatura = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LlenarGridViewCasos();
            dvTramitePrestamo.Visible = false;

            if (!IsPostBack)
            {
                CargarEstados();
                CargarEstadosJudiciales();
            }

        }

        protected void gvCasosJudicial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LlenarGridViewCasos();
            //CargarFormulario();
            //btnActualizarEstadoPrestamo.Visible = true;
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            if (e.CommandName == "Select")
            {
                // Obtener el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Obtener el valor de la columna deseada de la fila seleccionada (si es necesario)
                string numPretamoVar = gvCasosJudicial.Rows[index].Cells[3].Text; // Por ejemplo, aquí obtengo el valor de la primera celda
                string tipoVar = gvCasosJudicial.Rows[index].Cells[2].Text;
                string deudaInicialVar = gvCasosJudicial.Rows[index].Cells[5].Text;
                string saldoVar = gvCasosJudicial.Rows[index].Cells[6].Text;
                string adjudicadoPretamoVar = gvCasosJudicial.Rows[index].Cells[4].Text;
                string vencimientoVar = gvCasosJudicial.Rows[index].Cells[6].Text;
                string estadoVar = gvCasosJudicial.Rows[index].Cells[7].Text;
                string abogado = ""; // Inicializar la variable abogado
                //string codigoAbogado = "";
                string oficinaV = "";
                string oficialV = "";
                string adjudicadoV = "";
                string proVencimientoV = "";
                string saldoTransferidoV = "";
                string descripcionV = "";
                string comentarioV = "";
                string fechaMaquinaV = "";
                string fechaSistemaV = "";
                string tramiteV = "";
                string materiaV = "";
                string medidaCautelarV = "";
                string judicaturaV = "";
                string estadoTramiteV = "";
                string secuencialPrestamoV = "";
                string ultimoPagoV = "";
                string numCausaV = "";

                string query = @"
                    SELECT TOP(1)
	                    AB.CODIGO, 
                        AB.NOMBRE AS [ABOGADO], 
                        PA.SALDOTRANSFERIDO, 
                        PA.DESCRIPCION AS [DESCRIPCION],
                        CONVERT(VARCHAR, PA.FECHAMAQUINAINGRESO, 23) AS [FECHA MAQUINA],
                        CONVERT(VARCHAR, PA.FECHASISTEMAINGRESO, 23) AS [FECHA INGRESO], 
                        MC.NOMBRE AS [MEDIDA CAUTELAR], 
                        TT.NOMBRE AS [TRAMITE],	
                        TM.NOMBRE AS [MATERIA], 
                        DIV.NOMBRE AS [OFICINA],	
                        US.NOMBRE AS [OFICIAL],
                        CONVERT(VARCHAR, PM.FECHAADJUDICACION, 23) AS [ADJUDICADO], 
                        ISNULL(PT.COMENTARIO, '') AS [COMENTARIO],
                        CONVERT(VARCHAR, PIN.FECHAPROXIMOPAGO, 23) AS [PRO VENCIMIENTO],
                        TJ.NOMBRE AS [JUDICATURA], 
                        ETJ.NOMBRE AS [ACCIÓN DESARROLLADA], 
                        PA.SALDOTRANSFERIDO AS [SALDO TRANSFERIDO],
                        PM.SECUENCIAL AS [SECUENCIAL PRESTAMO],
                        PT.ESTAACTIVO AS [ACTIVO],
	                    PA.CODIGOABOGADO AS [CODIGOABOGADO],
	                    PA.CODIGOUSUARIOINGRESO AS [USUARIOINGRESO]
                    FROM [FBS_CARTERA].[PRESTAMOMAESTRO] PM
                    LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PM.SECUENCIAL = PT.SECUENCIALPRESTAMO
                    JOIN [FBS_CARTERA].[PRESTAMO_INFORMACIONADICIONAL] PIN ON PM.SECUENCIAL = PIN.SECUENCIALPRESTAMO
                    JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] PA ON PA.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    JOIN [FBS_COBRANZAS].[ABOGADO] AB ON AB.CODIGO = PA.CODIGOABOGADO
                    JOIN [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] MC ON MC.CODIGO = PA.CODIGOTIPOMEDCAUTELAR
                    JOIN [FBS_COBRANZAS].[TIPOTRAMITE] TT ON TT.CODIGO = PA.CODIGOTIPOTRAMITE
                    JOIN [FBS_COBRANZAS].[TIPOMATERIAJUICIO] TM ON TM.CODIGO = PA.CODIGOTIPOMATERIAJUI
                    JOIN [FBS_GENERALES].[DIVISION] DIV ON DIV.SECUENCIAL = PM.SECUENCIALOFICINA
                    JOIN [FBS_SEGURIDADES].[USUARIO] US ON US.CODIGO = PM.CODIGOUSUARIOOFICIAL
                    LEFT JOIN [FBS_CARTERA].[COMENTARIO] COM ON COM.SECUENCIALPRESTAMO = PM.SECUENCIAL
                    JOIN [FBS_COBRANZAS].[TIPOJUDICATURA] TJ ON TJ.CODIGO = PA.CODIGOTIPOJUDICATURA
                    LEFT JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ETJ ON ETJ.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                    WHERE PM.NUMEROPRESTAMO = '" + numPretamoVar + @"' AND PM.CODIGOUSUARIOOFICIAL NOT LIKE '%FPUEDMAGDEV.%'
                    ORDER BY PT.SECUENCIAL DESC; ";


                // Tu código para conectar a la base de datos y ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Verificar si hay filas en el resultado
                    {
                        string estado = reader["ACTIVO"].ToString();
                        //gridCheck.Checked = (estado == "True");

                        abogado = reader["ABOGADO"].ToString();

                        ListItem selectedAbogado = ddlAbogado.Items.FindByText(abogado);

                        if (selectedAbogado != null)
                        {
                            // Limpiar cualquier selección previa
                            foreach (ListItem item in ddlAbogado.Items)
                            {
                                item.Selected = false;
                            }

                            // Establecer la nueva selección
                            selectedAbogado.Selected = true;
                        }
                        tramiteV = reader["TRAMITE"].ToString();
                        ListItem selectedTramite = ddlTramite.Items.FindByText(tramiteV);
                        if (selectedTramite != null)
                        {
                            foreach (ListItem item in ddlTramite.Items)
                            {
                                item.Selected = false;
                            }

                            selectedTramite.Selected = true;
                        }

                        materiaV = reader["MATERIA"].ToString();
                        ListItem selectedMateria = ddlMateria.Items.FindByText(materiaV);
                        if (selectedMateria != null)
                        {
                            foreach (ListItem item in ddlMateria.Items)
                            {
                                item.Selected = false;
                            }

                            selectedMateria.Selected = true;
                        }

                        medidaCautelarV = reader["MEDIDA CAUTELAR"].ToString();
                        ListItem selectedMedidaCautelar = ddlMedidaCautelar.Items.FindByText(medidaCautelarV);
                        if (selectedMedidaCautelar != null)
                        {
                            foreach (ListItem item in ddlMedidaCautelar.Items)
                            {
                                item.Selected = false;
                            }

                            selectedMedidaCautelar.Selected = true;
                        }

                        judicaturaV = reader["JUDICATURA"].ToString();
                        ListItem selectedJudicatura = ddlJudicatura.Items.FindByText(judicaturaV);
                        if (selectedJudicatura != null)
                        {
                            foreach (ListItem item in ddlJudicatura.Items)
                            {
                                item.Selected = false;
                            }

                            selectedJudicatura.Selected = true;
                        }

                        estadoTramiteV = reader["ACCIÓN DESARROLLADA"].ToString();
                        ListItem selectedEstadoTramite = ddlAccion.Items.FindByText(estadoTramiteV);
                        if (selectedEstadoTramite != null)
                        {
                            foreach (ListItem item in ddlAccion.Items)
                            {
                                item.Selected = false;
                            }

                            selectedEstadoTramite.Selected = true;
                        }

                        secuencialPrestamoV = reader["SECUENCIAL PRESTAMO"].ToString();
                        secuencialPrestamo = secuencialPrestamoV;
                        codigoAbogado = reader["CODIGOABOGADO"].ToString();
                        codigousuario = reader["USUARIOINGRESO"].ToString();
                        oficialV = reader["OFICIAL"].ToString();
                        oficinaV = reader["OFICINA"].ToString();
                        adjudicadoV = reader["ADJUDICADO"].ToString();
                        proVencimientoV = reader["PRO VENCIMIENTO"].ToString();
                        saldoTransferidoV = reader["SALDO TRANSFERIDO"].ToString();
                        descripcionV = reader["DESCRIPCION"].ToString();
                        comentarioV = reader["COMENTARIO"].ToString();
                        fechaMaquinaV = reader["FECHA MAQUINA"].ToString();
                        fechaSistemaV = reader["FECHA INGRESO"].ToString();
                    }

                    reader.Close();
                }

                string queryUltimoPago = @"
                    SELECT CONVERT(VARCHAR, MAX(m.FECHA), 23) AS [FECHA]
                    FROM [FBS_CARTERA].[MOVIMIENTOPRESTAMOCOMP_CAR] mpcf
                    INNER JOIN [FBS_NEGOCIOSFINANCIEROS].[MOVIMIENTODETALLE] md ON mpcf.SECUENCIALMOVIMIENTODETALLE = md.SECUENCIAL
                    INNER JOIN [FBS_NEGOCIOSFINANCIEROS].[MOVIMIENTO] m ON md.SECUENCIALMOVIMIENTO = m.SECUENCIAL
                    INNER JOIN [FBS_NEGOCIOSFINANCIEROS].[TRANSACCION] t ON md.SECUENCIALTRANSACCION = t.SECUENCIAL
                    WHERE t.ESDEBITO = 'false'
                    AND mpcf.SECUENCIALPRESTAMO ='" + secuencialPrestamoV + @"'";

                string queryNumCausa = @"
                    SELECT pai.NUMEROCAUSA AS [NUM CAUSA] FROM [FBS_COBRANZAS].[PRESTAMOABOGADO] pa
                    JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO_INFORADICIONAL] pai
                    ON pa.SECUENCIAL=pai.SECUENCIALPRESTAMOABOGADO
                    WHERE pa.SECUENCIALPRESTAMO ='" + secuencialPrestamoV + @"'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryUltimoPago, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Verificar si hay filas en el resultado
                    {
                        ultimoPagoV = reader["FECHA"].ToString();
                    }

                    reader.Close();
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryNumCausa, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        numCausaV = reader["NUM CAUSA"].ToString();
                    }

                    reader.Close();
                }

                //CargarGridTramites(secuencialPrestamoV);
                litDiasDesdePrestamo.Text = ObtenerDiasDesdePrestamoJudicial(secuencialPrestamoV);

                string saldoActualCartera = SaldoActualCartera(secuencialPrestamoV);
                string interesExigibleCartera = ExigibleInteresCartera(secuencialPrestamoV);
                string moraExigibleCartera = ExigibleMoraCartera(secuencialPrestamoV);

                txtSaldoActualCartera.Text = saldoActualCartera;
                txtInteresExigibleCartera.Text = interesExigibleCartera;
                txtMoraExigibleCartera.Text = moraExigibleCartera;
                txtCausa.Text = numCausaV;
                txtOficial.Text = oficialV;
                txtOficina.Text = oficinaV;
                dtAdjudicado.Text = adjudicadoV;
                dtProxVencimiento.Text = proVencimientoV;
                txtTransferido.Text = saldoTransferidoV;
                txtDescripcion.Text = descripcionV;
                txtComentario.Text = comentarioV;
                dtFechaIngreso.Value = fechaMaquinaV;
                dtFechaSistema.Value = fechaSistemaV;
                dtUltimoPago.Text = ultimoPagoV;
                dvTramitePrestamo.Visible = true;
                txtNumPretamo.Text = numPretamoVar;
                Session["NumPretamo"] = numPretamoVar;
                txtTipo.Text = tipoVar;
                txtDeudaInicial.Text = deudaInicialVar;
                txtSaldoActual.Text = saldoVar;
            }
        }

        protected string SaldoActualCartera(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";

            // Definir la consulta SQL
            string query = @"
        SELECT FORMAT((ROUND([VALORCALCULADO], 2)), 'N2') AS SALDO_ACTUAL
        FROM [FBS_CARTERA].[PRESTAMOCOMPONENTE_CARTERA]
        WHERE SECUENCIALPRESTAMO = @SecuencialPrestamo
        AND SECUENCIALCOMPONENTECARTERA = '75';";

            // Utilizar SqlConnection y SqlCommand para ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);

                try
                {
                    connection.Open();
                    respuesta = command.ExecuteScalar()?.ToString() ?? "0.00"; // Si el resultado es nulo, devolver "0.00"
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    respuesta = "Error: " + ex.Message;
                }
            }

            return respuesta;
        }
        protected string ExigibleInteresCartera(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";

            // Definir la consulta SQL
            string query = @"
        SELECT FORMAT((ROUND([VALORCALCULADO], 2)), 'N2') AS SALDO_ACTUAL
        FROM [FBS_CARTERA].[PRESTAMOCOMPONENTE_CARTERA]
        WHERE SECUENCIALPRESTAMO = @SecuencialPrestamo
        AND SECUENCIALCOMPONENTECARTERA = '47';";

            // Utilizar SqlConnection y SqlCommand para ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);

                try
                {
                    connection.Open();
                    respuesta = command.ExecuteScalar()?.ToString() ?? "0.00"; // Si el resultado es nulo, devolver "0.00"
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    respuesta = "Error: " + ex.Message;
                }
            }

            return respuesta;
        }
        protected string ExigibleMoraCartera(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";

            // Definir la consulta SQL
            string query = @"
        SELECT FORMAT((ROUND([VALORCALCULADO], 2)), 'N2') AS SALDO_ACTUAL
        FROM [FBS_CARTERA].[PRESTAMOCOMPONENTE_CARTERA]
        WHERE SECUENCIALPRESTAMO = @SecuencialPrestamo
        AND SECUENCIALCOMPONENTECARTERA = '76';";

            // Utilizar SqlConnection y SqlCommand para ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);

                try
                {
                    connection.Open();
                    respuesta = command.ExecuteScalar()?.ToString() ?? "0.00"; // Si el resultado es nulo, devolver "0.00"
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    respuesta = "Error: " + ex.Message;
                }
            }

            return respuesta;
        }

        private string ObtenerDiasDesdePrestamoJudicial(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query = @"
            SELECT 
                DATEDIFF(DAY, FECHASISTEMAINGRESO, GETDATE()) AS DiasDesdePrestamoJudicial
            FROM 
                [FBS_COBRANZAS].[PRESTAMOABOGADO]
            WHERE SECUENCIALPRESTAMO = @SecuencialPrestamo";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            return "No se encontró el registro.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones según sea necesario
                return $"Error: {ex.Message}";
            }
        }


        protected void gvEstadosJudiciales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string idTramiteJudicial = e.CommandArgument.ToString();
                string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] SET NUMEROVERIFICADOR = '0' WHERE SECUENCIAL = @Secuencial";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Secuencial", idTramiteJudicial);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            //VaciarGridView();
                            //CargarGridTramites(secuencialPrestamo);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error al actualizar el registro: " + ex.Message);
                        }
                    }
                }
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
            [NOMBRE SOCIO];";

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
            [NOMBRE SOCIO];";

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
                        [NOMBRE SOCIO];";

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
                        [NOMBRE SOCIO];";




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
                        [NOMBRE SOCIO];";

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
                        [NOMBRE SOCIO];";




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
                    [NOMBRE SOCIO];";
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
                    [NOMBRE SOCIO];";
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