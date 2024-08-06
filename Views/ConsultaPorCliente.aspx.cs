using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MonitorJudicial.Controllers;

namespace MonitorJudicial
{
    public partial class ConsultaPorCliente : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                dvTramitePrestamo.Visible = false;
                txtNombresDiv.Visible = false;
                //CargarFormulario();
            }

        }

        protected void CargarFormulario()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string queryAbogado = "SELECT NOMBRE FROM [FBS_COBRANZAS].[ABOGADO] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
            string queryTramite = "SELECT NOMBRE FROM [FBS_COBRANZAS].[TIPOTRAMITE] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
            string queryMateria = "SELECT NOMBRE FROM [FBS_COBRANZAS].[TIPOMATERIAJUICIO] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
            string queryMedidaCautelar = "SELECT NOMBRE FROM [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
            string queryJudicatura = "SELECT NOMBRE FROM [FBS_COBRANZAS].[TIPOJUDICATURA] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";
            string queryEstadoTramite = "SELECT NOMBRE\r\nFROM [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] \r\nWHERE ESTAACTIVO = '1'\r\nORDER BY CASE CODIGO\r\n    WHEN '19' THEN 1\r\n    WHEN '07' THEN 2\r\n    WHEN '16' THEN 3\r\n    WHEN '25' THEN 4\r\n    WHEN '09' THEN 5\r\n    WHEN '26' THEN 6\r\n    WHEN '22' THEN 7\r\n    WHEN '14' THEN 8\r\n    WHEN '15' THEN 9\r\n    WHEN '27' THEN 10\r\n    WHEN '12' THEN 11\r\n    WHEN '06' THEN 12\r\n    WHEN '28' THEN 13\r\n    WHEN '21' THEN 14\r\n    WHEN '03' THEN 15\r\n    WHEN '01' THEN 16\r\n    WHEN '02' THEN 17\r\n    WHEN '04' THEN 18\r\n    WHEN '05' THEN 19\r\n    WHEN '08' THEN 20\r\n    WHEN '10' THEN 21\r\n    WHEN '11' THEN 22\r\n    WHEN '13' THEN 23\r\n    WHEN '17' THEN 24\r\n    WHEN '18' THEN 25\r\n    WHEN '20' THEN 26\r\n    WHEN '23' THEN 27\r\n    WHEN '30' THEN 28\r\n    WHEN '24' THEN 29\r\nEND;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryAbogado, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreAbogado = reader["NOMBRE"].ToString();
                        ddlAbogado.Items.Add(new ListItem(nombreAbogado));
                    }

                    reader.Close();
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryTramite, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreTramite = reader["NOMBRE"].ToString();
                        ddlTramite.Items.Add(new ListItem(nombreTramite));
                    }

                    reader.Close();
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryMateria, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreMateria = reader["NOMBRE"].ToString();
                        ddlMateria.Items.Add(new ListItem(nombreMateria));
                    }

                    reader.Close();
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryMedidaCautelar, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreMedidaCautelar = reader["NOMBRE"].ToString();
                        ddlMedidaCautelar.Items.Add(new ListItem(nombreMedidaCautelar));
                    }

                    reader.Close();
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryJudicatura, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreJudicatura = reader["NOMBRE"].ToString();
                        ddlJudicatura.Items.Add(new ListItem(nombreJudicatura));
                    }

                    reader.Close();
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryEstadoTramite, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreEstadoTramite = reader["NOMBRE"].ToString();
                        ddlAccion.Items.Add(new ListItem(nombreEstadoTramite));
                    }

                    reader.Close();
                }
            }
        }

        protected void LlenarGridViewCliente(string numeroCliente)
        {
            txtNombresDiv.Visible = true;
            Controllers.PrestamosController.LlenarGridViewCliente(numeroCliente, gvPrestamos, txtNombres);
        }

        protected void LlenarGridViewCedula(string numeroCedula)
        {
            txtNombresDiv.Visible = true;
            Controllers.PrestamosController.LlenarGridViewCedula(numeroCedula, gvPrestamos, txtNombres);
        }

        protected void LlenarGridViewCaso(string numeroCaso)
        {
            txtNombresDiv.Visible = true;
            Controllers.PrestamosController.LlenarGridViewCaso(numeroCaso, gvPrestamos, txtNombres);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Limpiar las variables de sesión
            Session["Nombres"] = null;
            Session["Rol"] = null;
            Session["CodigoAbogado"] = null;

            // O puedes usar Session.Clear() para limpiar todas las variables de sesión
            // Session.Clear();

            // Redirigir a la página de inicio de sesión
            Response.Redirect("Login.aspx");
        }

        protected void gvPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CargarFormulario();
            btnActualizarEstadoPrestamo.Visible = true;
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            if (e.CommandName == "Select")
            {
                // Obtener el índice de la fila seleccionada
                int index = Convert.ToInt32(e.CommandArgument);

                // Obtener el valor de la columna deseada de la fila seleccionada (si es necesario)
                string numPretamoVar = gvPrestamos.Rows[index].Cells[1].Text; // Por ejemplo, aquí obtengo el valor de la primera celda
                string tipoVar = gvPrestamos.Rows[index].Cells[2].Text;
                string deudaInicialVar = gvPrestamos.Rows[index].Cells[3].Text;
                string saldoVar = gvPrestamos.Rows[index].Cells[4].Text;
                string adjudicadoPretamoVar = gvPrestamos.Rows[index].Cells[5].Text;
                string vencimientoVar = gvPrestamos.Rows[index].Cells[6].Text;
                string estadoVar = gvPrestamos.Rows[index].Cells[7].Text;
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
                    WHERE PM.NUMEROPRESTAMO = '" + numPretamoVar + @"'
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

                CargarGridTramites(secuencialPrestamoV);
                litDiasDesdePrestamo.Text = ObtenerDiasDesdePrestamoJudicial(secuencialPrestamoV);

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
                txtTipo.Text = tipoVar;
                txtDeudaInicial.Text = deudaInicialVar;
                txtSaldoActual.Text = saldoVar;
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
                            VaciarGridView();
                            CargarGridTramites(secuencialPrestamo);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error al actualizar el registro: " + ex.Message);
                        }
                    }
                }
            }
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
        protected void CargarGridTramites(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string secuencialPrestamoV = secuencialPrestamo;
            string queryEstados = @"
	                    SELECT 
                            PT.SECUENCIAL AS [ID],
                            ISNULL(EJ.NOMBRE, '') AS [TRÁMITE], 
                            ISNULL(PT.COMENTARIO, '') AS [COMENTARIO],
                            ISNULL(PT.ESTAACTIVO, '') AS [ACTIVO], 
                            ISNULL(CONVERT(VARCHAR, PT.FECHASISTEMA, 105), '') AS [FECHA ESTADO JUDICIAL],                            
                            ISNULL((SELECT TOP 1 -DATEDIFF(DAY, PT.FECHASISTEMA, PT2.FECHASISTEMA) 
                                     FROM [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT2 
                                     WHERE PT2.SECUENCIALPRESTAMO = PA.SECUENCIALPRESTAMO 
                                     AND PT2.FECHASISTEMA < PT.FECHASISTEMA
                                     ORDER BY PT2.FECHASISTEMA DESC), '') AS [DÍAS DIFERENCIA],
                            ISNULL(CAST(DATEDIFF(DAY, PT.FECHASISTEMA, GETDATE()) AS VARCHAR), '') AS [DÍAS HASTA HOY]
                        FROM 
                            [FBS_COBRANZAS].[PRESTAMOABOGADO] PA
                            LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = PA.SECUENCIALPRESTAMO
                            LEFT JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] EJ ON PT.CODIGOESTADOTRAMITEDEMJUD = EJ.CODIGO
                        WHERE 
                            PA.SECUENCIALPRESTAMO = '" + secuencialPrestamoV + @"'
                            AND PT.NUMEROVERIFICADOR <> 0
                        ORDER BY 
                            PT.FECHASISTEMA; ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryEstados, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Asignar datos a la GridView
                gvEstadosJudiciales.DataSource = dataTable;
                gvEstadosJudiciales.DataBind();
            }
        }
        protected void VaciarGridView()
        {
            // Asigna un DataSource vacío y actualiza el GridView
            gvEstadosJudiciales.DataSource = null;
            gvEstadosJudiciales.DataBind();
        }
        protected string UpdatePrestamoEstado(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            string query = "UPDATE [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] SET [ESTAACTIVO] = 0 WHERE [SECUENCIALPRESTAMO] = @SecuencialPrestamo";

            string respuesta = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verifica si se actualizó alguna fila
                    if (rowsAffected > 0)
                    {
                        // Éxito: se actualizó al menos una fila
                        respuesta = "OK";
                    }
                    else
                    {
                        // No se actualizó ninguna fila
                        respuesta = "ERROR";
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    respuesta = "ERROR";
                    // Puedes registrar el error o mostrar un mensaje adecuado al usuario
                }
            }
            return respuesta;
        }
        protected void ConsultaPrestamoJudicial(string secuencialPrestamo)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;

            string query = "SELECT * FROM [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] WHERE [SECUENCIALPRESTAMO] = @SecuencialPrestamo AND ESTAACTIVO = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo); // Agregar el parámetro

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    codigoabogado = reader["CODIGOABOGADO"].ToString();
                    numeroverificador = reader["NUMEROVERIFICADOR"].ToString();
                    codigousuario = reader["CODIGOUSUARIO"].ToString();

                    // Aquí puedes usar las variables codigoabogado, numeroverificador y codigousuario
                    // por ejemplo, asignándolas a controles de la interfaz de usuario o a otras variables de clase
                }

                reader.Close();
            }

        }
        protected string UpdatePrestamoDescripcion(string secuencialPrestamo, string descripcion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string medicaCautelarV = ddlMedidaCautelar.SelectedValue.Trim();
            string queryMedidaCautelar = @"SELECT NOMBRE, CODIGO FROM [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] WHERE NOMBRE = @medicaCautelarV;";
            string judicaturaV = ddlJudicatura.SelectedValue.Trim();
            string queryJudicatura = @"SELECT CODIGO FROM [FBS_COBRANZAS].[TIPOJUDICATURA] WHERE NOMBRE = @judicaturaV;";


            string codigoJudicatura = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryJudicatura, connection))
                {
                    command.Parameters.AddWithValue("@judicaturaV", judicaturaV);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //nombreResultado = reader["NOMBRE"].ToString();
                                codigoJudicatura = reader["CODIGO"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            //string nombreResultado = null;
            string codigoResultado = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryMedidaCautelar, connection))
                {
                    command.Parameters.AddWithValue("@medicaCautelarV", medicaCautelarV);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //nombreResultado = reader["NOMBRE"].ToString();
                                codigoResultado = reader["CODIGO"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            string respuesta = "";
            string query = @"
            UPDATE [FBS_COBRANZAS].[PRESTAMOABOGADO]
            SET DESCRIPCION = @Descripcion,
            CODIGOTIPOMEDCAUTELAR = @codigoResultado,
            CODIGOTIPOJUDICATURA = @codigoJudicatura
            WHERE SECUENCIALPRESTAMO = @SecuencialPrestamo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialPrestamo);
                command.Parameters.AddWithValue("@Descripcion", descripcion);
                command.Parameters.AddWithValue("@codigoResultado", codigoResultado);
                command.Parameters.AddWithValue("@codigoJudicatura", codigoJudicatura);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    // Verifica si se actualizó alguna fila
                    if (rowsAffected > 0)
                    {
                        // Éxito: se actualizó al menos una fila
                        respuesta = "OK";
                    }
                    else
                    {
                        // No se actualizó ninguna fila
                        respuesta = "ERROR";
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine("Error: " + ex.Message);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Ocurrió un error al actualizar la descripción.');", true);
                    // Puedes registrar el error o mostrar un mensaje adecuado al usuario
                }
            }
            return respuesta;
        }

        
        protected string ConsultarCodigoTramite(string codigoestadotramitedemjud)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";
            string query = @"
            SELECT CODIGO 
            FROM [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] 
            WHERE NOMBRE = @codigoestadotramitedemjud";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@codigoestadotramitedemjud", codigoestadotramitedemjud);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        respuesta = reader["CODIGO"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = "Error al obtener el código";
                }
            }

            return respuesta;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            dvTramitePrestamo.Visible = false;
            if (rbCedula.Checked)  // Verificar si el radio button 'rbCedula' está seleccionado
            {
                LlenarGridViewCedula(idConsulta.Value);
            }
            if (rbCliente.Checked)
            {
                LlenarGridViewCliente(idConsulta.Value);
            }
            if (rbCaso.Checked)
            {
                LlenarGridViewCaso(idConsulta.Value);
            }
        }
        static string descripcion;
        protected void btnActualizarEstadoPrestamo_Click(object sender, EventArgs e)
        {
            ddlAccion.Enabled = true;
            ddlAccion.SelectedIndex = 0;
            ddlMedidaCautelar.Enabled = true;
            ddlJudicatura.Enabled = true;
            //ddlMedidaCautelar.Enabled = true;
            //ddlMedidaCautelar.SelectedIndex = 0;
            txtComentario.ReadOnly = false;
            txtComentario.Text = string.Empty;
            //txtDescripcion.ReadOnly = false;
            //txtDescripcion.Text = string.Empty;
            btnActualizarEstadoPrestamo.Visible = false;
            btnGuardarEstadoPrestamo.Visible = true;
            btnCancelarEstadoPrestamo.Visible = true;
            dtFechaSistema.Value = DateTime.Now.ToString("yyyy-MM-dd");
            dtFechaIngreso.Value = DateTime.Now.ToString("yyyy-MM-dd");

            string script = @"
        <script type='text/javascript'>
            var highlightColor = '#FEF0BD';
            document.getElementById('" + txtComentario.ClientID + @"').style.backgroundColor = highlightColor;
            document.getElementById('" + ddlAccion.ClientID + @"').style.backgroundColor = highlightColor;
            document.getElementById('" + txtComentario.ClientID + @"').addEventListener('focus', function() {
                this.style.backgroundColor = '';
            });

            document.getElementById('" + ddlAccion.ClientID + @"').addEventListener('focus', function() {
                this.style.backgroundColor = '';
            });
document.getElementById('" + ddlMedidaCautelar.ClientID + @"').style.backgroundColor = highlightColor;
            document.getElementById('" + ddlJudicatura.ClientID + @"').style.backgroundColor = highlightColor;
            document.getElementById('" + ddlMedidaCautelar.ClientID + @"').addEventListener('focus', function() {
                this.style.backgroundColor = '';
            });

            document.getElementById('" + ddlJudicatura.ClientID + @"').addEventListener('focus', function() {
                this.style.backgroundColor = '';
            });

        </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "HighlightFields", script);
        }

        protected string GuardarEstadoPrestamo(int secuencialprestamoV, string codigoEstadoJudicialV, string codigoabogadoV, string comentarioV, bool estaactivoV, int numeroverificadorV, string codigousuarioV, DateTime fechasistemaV, DateTime fechamaquinaV)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string respuesta = "";
            string query = @"
            INSERT INTO [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE]
            ([SECUENCIALPRESTAMO]
            ,[CODIGOESTADOTRAMITEDEMJUD]
            ,[CODIGOABOGADO]
            ,[COMENTARIO]
            ,[ESTAACTIVO]
            ,[NUMEROVERIFICADOR]
            ,[CODIGOUSUARIO]
            ,[FECHASISTEMA]
            ,[FECHAMAQUINA])
            OUTPUT INSERTED.SECUENCIAL
            VALUES
            (@SecuencialPrestamo
            ,@CodigoEstadoJudicial
            ,@CodigoAbogado
            ,@Comentario
            ,@EstaActivo
            ,@NumeroVerificador
            ,@CodigoUsuario
            ,@FechaSistema
            ,@FechaMaquina)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialprestamoV);
                command.Parameters.AddWithValue("@CodigoEstadoJudicial", codigoEstadoJudicialV);
                command.Parameters.AddWithValue("@CodigoAbogado", codigoabogadoV);
                command.Parameters.AddWithValue("@Comentario", comentarioV);
                command.Parameters.AddWithValue("@EstaActivo", estaactivoV);
                command.Parameters.AddWithValue("@NumeroVerificador", numeroverificadorV);
                command.Parameters.AddWithValue("@CodigoUsuario", codigousuarioV);
                command.Parameters.AddWithValue("@FechaSistema", fechasistemaV);
                command.Parameters.AddWithValue("@FechaMaquina", fechamaquinaV);

                try
                {
                    connection.Open();
                    int secuencialInsertado = (int)command.ExecuteScalar(); // Obtener el valor insertado
                    //GuardarValoresJudiciales(secuencialprestamoV, secuencialInsertado, txtConcepto.Text, Double.Parse(txtValor.Text), ddlAccion.SelectedValue.ToString());
                    // Aquí puedes hacer lo que necesites con el secuencialInsertado
                    respuesta = "OK";
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    Console.WriteLine("Error: " + ex.Message);
                    respuesta = "ERROR";
                    // Puedes registrar el error o mostrar un mensaje adecuado al usuario
                }
            }
            return respuesta;
        }
        //protected string GuardarValoresJudiciales(int secuencialprestamoV, int secuencialJudicialV, string conceptoV, double valorV, string accionJudicialV)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
        //    string respuesta = "";
        //    string query = @"
        //    INSERT INTO [FBS_COBRANZAS].[VALORES_POR_ACCION_JUDICIAL]
        //   ([SECUENCIALPRESTAMO]
        //   ,[SECUENCIAL_TRAM_JUDICIAL]
        //   ,[CONCEPTO]  
        //   ,[VALOR]
        //   ,[ACCION_JUDICIAL]) 
        //    VALUES
        //    (@SecuencialPrestamo
        //    ,@Secuencial_Tram_Judicial
        //    ,@Concepto
        //    ,@Valor
        //    ,@Accion_Judicial)";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@SecuencialPrestamo", secuencialprestamoV);
        //        command.Parameters.AddWithValue("@Secuencial_Tram_Judicial", secuencialJudicialV);
        //        command.Parameters.AddWithValue("@Concepto", conceptoV);
        //        command.Parameters.AddWithValue("@Valor", valorV);
        //        command.Parameters.AddWithValue("@Accion_Judicial", accionJudicialV);

        //        try
        //        {
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            respuesta = "OK";
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            Console.WriteLine("Error: " + ex.Message);
        //            respuesta = "ERROR";
        //            // Puedes registrar el error o mostrar un mensaje adecuado al usuario
        //        }
        //    }
        //    return respuesta;
        //}

        protected void btnGuardarEstadoPrestamo_Click(object sender, EventArgs e)
        {
            string textComentario=txtComentario.Text;

            if(string.IsNullOrEmpty(textComentario))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error! Ingrese un Comentario!');", true);
                return;
            }
            else
            {
                string descripcion = ddlAccion.SelectedValue.Trim();
                //Este método trae la información de la tabla [PRESTAMODEMANDAJUDICIALTRAMITE] 
                ConsultaPrestamoJudicial(secuencialPrestamo);

                string medicaCautelarV = ddlMedidaCautelar.SelectedValue.Trim();
                string medidaCau = ddlMedidaCautelar.SelectedIndex.ToString();
                string medidasCau = ddlMedidaCautelar.SelectedItem.ToString();
                //string judicaturaV = ddlJudicatura.SelectedIndex.ToString();

                int secuencialprestamoV = int.Parse(secuencialPrestamo);
                string codigoEstadoJudicialV = ConsultarCodigoTramite(descripcion);
                string codigoabogadoV = codigoAbogado;
                string comentarioV = txtComentario.Text.Trim().ToUpper();
                bool estaactivoV = true;
                int numeroverificadorV = 1;//int.Parse(numeroverificador);
                string codigousuarioV = codigousuario;
                DateTime fechasistemaV = DateTime.Parse(dtFechaSistema.Value);
                DateTime fechamaquinaV = DateTime.Parse(dtFechaIngreso.Value);

                try
                {
                    string actualizadoEstado = UpdatePrestamoEstado(secuencialPrestamo);
                    //if (actualizadoEstado.Equals("OK"))
                    //{
                    string actualizadoDescripcion = UpdatePrestamoDescripcion(secuencialPrestamo, descripcion);
                    if (actualizadoDescripcion.Equals("OK"))
                    {

                        string guardado = GuardarEstadoPrestamo(secuencialprestamoV, codigoEstadoJudicialV, codigoabogadoV, comentarioV, estaactivoV, numeroverificadorV, codigousuarioV, fechasistemaV, fechamaquinaV);
                        if (guardado.Equals("OK"))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Estado Judicial Actualizado!');", true);
                            VaciarGridView();
                            CargarGridTramites(secuencialPrestamo);
                            //Cuando se h confirmdo el guardado
                            ddlAccion.Enabled = false;
                            ddlMedidaCautelar.Enabled = false;
                            ddlJudicatura.Enabled = false;
                            txtComentario.ReadOnly = true;
                            txtDescripcion.Text = ddlAccion.SelectedValue.Trim();
                            btnActualizarEstadoPrestamo.Visible = true;
                            btnGuardarEstadoPrestamo.Visible = false;
                            btnCancelarEstadoPrestamo.Visible = false;
                            //CargarFormulario();
                            ///
                            //Response.Redirect(Request.RawUrl);


                            //string script = "alert('Estado Judicial Actualizado!');";
                            //script += "window.location = '" + Request.RawUrl + "';"; // Redirige a la URL actual después de la alerta
                            //ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);


                            return;
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error! No se pudo actualizar!');", true);
                            return;
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error! No se pudo actualizar!');", true);
                        return;
                    }
                    //}
                    //else
                    //{
                    //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error! No se pudo actualizar!');", true);
                    //    return;
                    //}
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error! No se pudo actualizar!');", true);
                    return;
                }
            }
        }
        protected void btnCancelarEstadoPrestamo_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Estado Judicial Actualizado!');", true);
            VaciarGridView();
            CargarGridTramites(secuencialPrestamo);
            //Cuando se h confirmdo el guardado
            ddlAccion.Enabled = false;
            ddlMedidaCautelar.Enabled = false;
            ddlJudicatura.Enabled = false;
            ddlAccion.SelectedIndex = 0;
            txtComentario.ReadOnly = true;
            //txtDescripcion.Text = ddlAccion.SelectedValue.Trim();
            btnActualizarEstadoPrestamo.Visible = true;
            btnGuardarEstadoPrestamo.Visible = false;
            btnCancelarEstadoPrestamo.Visible = false;
        }
    }
}