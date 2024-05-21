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
            string queryEstadoTramite = "SELECT NOMBRE FROM [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";

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
            Controllers.PrestamosController.LlenarGridViewCliente(numeroCliente, gvPrestamos,txtNombres);
        }

        protected void LlenarGridViewCedula(string numeroCedula)
        {
            txtNombresDiv.Visible = true;
            Controllers.PrestamosController.LlenarGridViewCedula(numeroCedula, gvPrestamos, txtNombres);
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
                    SELECT 
                        ab.CODIGO, 
                        ab.NOMBRE AS [ABOGADO], 
                        pa.SALDOTRANSFERIDO, 
                        pa.DESCRIPCION AS [DESCRIPCION],
                        CONVERT(VARCHAR, pa.FECHAMAQUINAINGRESO, 23) AS [FECHA MAQUINA],
                        CONVERT(VARCHAR, pa.FECHASISTEMAINGRESO, 23) AS [FECHA INGRESO], 
                        mc.NOMBRE AS [MEDIDA CAUTELAR], 
                        tt.NOMBRE AS [TRAMITE],	
                        tm.NOMBRE AS [MATERIA], 
                        d.NOMBRE AS [OFICINA],	
                        u.NOMBRE AS [OFICIAL],
                        CONVERT(VARCHAR, pm.FECHAADJUDICACION, 23) AS [ADJUDICADO], 
                        ISNULL(pt.COMENTARIO, '') AS [COMENTARIO],
                        CONVERT(VARCHAR, pi.FECHAPROXIMOPAGO, 23) AS [PRO VENCIMIENTO],
                        tj.NOMBRE AS [JUDICATURA], 
                        ej.NOMBRE AS [ACCIÓN DESARROLLADA], 
                        pa.SALDOTRANSFERIDO AS [SALDO TRANSFERIDO],
                        pm.SECUENCIAL AS [SECUENCIAL PRESTAMO],
	                    pt.ESTAACTIVO AS [ACTIVO]
                    FROM 
                        [FBS_COBRANZAS].[ABOGADO] ab
                    JOIN 
                        [FBS_COBRANZAS].[ABOGADOOFICINA] ao ON ab.CODIGO = ao.CODIGOABOGADO
                    JOIN 
                        [FBS_COBRANZAS].[PRESTAMOABOGADO] pa ON ab.CODIGO = pa.CODIGOABOGADO
                    JOIN 
                        [FBS_CARTERA].[PRESTAMOMAESTRO] pm ON pm.SECUENCIAL = pa.SECUENCIALPRESTAMO
                    JOIN 
                        [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] mc ON pa.CODIGOTIPOMEDCAUTELAR = mc.CODIGO
                    JOIN 
                        [FBS_COBRANZAS].[TIPOTRAMITE] tt ON pa.CODIGOTIPOTRAMITE = tt.CODIGO
                    JOIN 
                        [FBS_COBRANZAS].[TIPOMATERIAJUICIO] tm ON pa.CODIGOTIPOMATERIAJUI = tm.CODIGO
                    JOIN 
                        [FBS_GENERALES].[DIVISION] d ON pm.SECUENCIALOFICINA = d.SECUENCIAL
                    JOIN 
                        [FBS_SEGURIDADES].[USUARIO] u ON pm.CODIGOUSUARIOOFICIAL = u.CODIGO
                    LEFT JOIN 
                        [FBS_CARTERA].[COMENTARIO] c ON pm.SECUENCIAL = c.SECUENCIALPRESTAMO
                    JOIN 
                        [FBS_CARTERA].[PRESTAMO_INFORMACIONADICIONAL] pi ON pm.SECUENCIAL = pi.SECUENCIALPRESTAMO
                    JOIN 
                        [FBS_COBRANZAS].[TIPOJUDICATURA] tj ON pa.CODIGOTIPOJUDICATURA = tj.CODIGO
                    LEFT JOIN 
                        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] pt ON pm.SECUENCIAL = pt.SECUENCIALPRESTAMO
                    JOIN 
                        [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ej ON pt.CODIGOESTADOTRAMITEDEMJUD = ej.CODIGO
                    WHERE 
                        pm.NUMEROPRESTAMO ='" + numPretamoVar + @"'
                        AND ab.SECUENCIALEMPRESA = pm.SECUENCIALEMPRESA
                        AND ao.SECUENCIALOFICINA = pm.SECUENCIALOFICINA
                        AND pt.ESTAACTIVO='1'; ";


                // Tu código para conectar a la base de datos y ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Verificar si hay filas en el resultado
                    {
                        string estado = reader["ACTIVO"].ToString();
                        gridCheck.Checked = (estado == "True");

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

                string queryEstados = @"
                    	SELECT 
                        ej.NOMBRE AS [ESTADO TRÁMITE], 
                        pt.COMENTARIO AS [COMENTARIO], 
                        FORMAT(pt.FECHAMAQUINA, 'yyyy-MM-dd') AS [FECHAMAQUINA], 
                        pt.ESTAACTIVO AS [ACTIVO],
                        ISNULL(CAST(DATEDIFF(DAY, LAG(pt.FECHAMAQUINA) OVER (PARTITION BY pt.SECUENCIALPRESTAMO ORDER BY pt.FECHAMAQUINA), pt.FECHAMAQUINA) AS VARCHAR), '') AS [DIFERENCIA DÍAS],
                        CASE 
                            WHEN pt.FECHAMAQUINA = (SELECT MAX(pt2.FECHAMAQUINA) 
                                                    FROM [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] pt2 
                                                    WHERE pt2.SECUENCIALPRESTAMO = pt.SECUENCIALPRESTAMO)
                            THEN CAST(DATEDIFF(DAY, pt.FECHAMAQUINA, GETDATE()) AS VARCHAR)
                            ELSE ''
                        END AS [DÍAS HASTA HOY]
                    FROM 
                        [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] pt
                        INNER JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ej 
                            ON pt.CODIGOESTADOTRAMITEDEMJUD = ej.CODIGO
                    WHERE 
                        pt.SECUENCIALPRESTAMO = '" + secuencialPrestamoV + @"'
                    ORDER BY 
                        pt.FECHAMAQUINA";

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (rbCedula.Checked)  // Verificar si el radio button 'rbCedula' está seleccionado
            {
                LlenarGridViewCedula(idConsulta.Value);
            }
            else
            {
                LlenarGridViewCliente(idConsulta.Value);
            }
        }
        static string descripcion;
        protected void btnActualizarEstadoPrestamo_Click(object sender, EventArgs e)
        {
            ddlAccion.Disabled = false;
            txtComentario.ReadOnly = false;
            txtDescripcion.ReadOnly = false;
            btnActualizarEstadoPrestamo.Visible = false;
            btnGuardarEstadoPrestamo.Visible = true;
            btnCancelarEstadoPrestamo.Visible = true;            
            dtFechaSistema.Value = DateTime.Now.ToString("yyyy-MM-dd");
            dtFechaIngreso.Value = DateTime.Now.ToString("yyyy-MM-dd");

        

            string variable = txtDescripcion.Text;
            //ddlTramite.Enabled = true;
            ddlMedidaCautelar.Enabled = true;
            //string valor = ddlMedidaCautelar.SelectedValue;
            //string valor2 = ddlMedidaCautelar.Text;
            //string valor3 = ddlTramite.SelectedItem.ToString();
            //string valor4 = ddlTramite.SelectedValue.ToString();
            //string valor6 = ddlMedidaCautelar.SelectedItem.ToString();
            //string valor7 = ddlMedidaCautelar.SelectedValue.ToString();
            //string valor5 = ddlTramite.SelectedIndex.ToString();
            
            //descripcion = txtDescripcion.Text;


        }
        protected void btnGuardarEstadoPrestamo_Click(object sender, EventArgs e)
        {            
            string valor6 = txtDescripcion.Text.Trim();
            string comentario = txtComentario.Text.Trim();
            string valor8 = ddlMedidaCautelar.SelectedItem.ToString();
            //string valor0 = ddlAccion.SelectedItem.ToString();
            string valor7 = ddlMedidaCautelar.SelectedValue.ToString();

            //if (valor4 == "")
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Por favor seleccione un trámite válido.');", true);
            //    return;
            //}
        }


    }
}