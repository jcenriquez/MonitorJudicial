﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
                divTramitePrestamo.Visible = false;
                CargarFormulario();
            }
        }

        protected void CargarFormulario()
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query = "SELECT NOMBRE FROM [FBS_COBRANZAS].[ABOGADO] WHERE ESTAACTIVO='1' ORDER BY NOMBRE";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreAbogado = reader["NOMBRE"].ToString();
                        inlineAbogado.Items.Add(new ListItem(nombreAbogado));
                    }

                    reader.Close();
                }
            }
        }
        protected void LlenarGridViewCliente(string numeroCliente)
        {
            Controllers.PrestamosController.LlenarGridViewCliente(numeroCliente, gvPrestamos);
        }

        protected void LlenarGridViewCedula(string numeroCedula)
        {
            Controllers.PrestamosController.LlenarGridViewCedula(numeroCedula, gvPrestamos);
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

        protected void gvPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
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

                string query = @"
                SELECT 
	ab.CODIGO, ab.NOMBRE AS [ABOGADO], pa.SALDOTRANSFERIDO, pa.DESCRIPCION AS [DESCRIPCION],
	CONVERT(VARCHAR, pa.FECHAMAQUINAINGRESO, 23) AS [FECHA MAQUINA],
	CONVERT(VARCHAR, pa.FECHASISTEMAINGRESO, 23) AS [FECHA INGRESO], mc.NOMBRE AS [MEDIDA CAUTELAR], 
	tt.NOMBRE AS [TRAMITE],	tm.NOMBRE AS [MATERIA], d.NOMBRE AS [OFICINA],	u.NOMBRE AS [OFICIAL],
	CONVERT(VARCHAR, pm.FECHAADJUDICACION, 23) AS [ADJUDICADO], ISNULL(pt.COMENTARIO, '') AS [COMENTARIO],
	CONVERT(VARCHAR, pi.FECHAPROXIMOPAGO, 23) AS [PRO VENCIMIENTO],
	tj.NOMBRE AS [JUDICATURA], ej.NOMBRE AS [ACCIÓN DESARROLLADA], pa.SALDOTRANSFERIDO AS [SALDO TRANSFERIDO]
	FROM [FBS_COBRANZAS].[ABOGADO] ab
  JOIN [FBS_COBRANZAS].[ABOGADOOFICINA] ao
  ON ab.CODIGO=ao.CODIGOABOGADO
  JOIN [FBS_COBRANZAS].[PRESTAMOABOGADO] pa
  ON ab.CODIGO=pa.CODIGOABOGADO
  JOIN [FBS_CARTERA].[PRESTAMOMAESTRO] pm
  ON pm.SECUENCIAL=pa.SECUENCIALPRESTAMO
  JOIN [FBS_COBRANZAS].[TIPOMEDIDACAUTELAR] mc
  ON pa.CODIGOTIPOMEDCAUTELAR=mc.CODIGO
  JOIN [FBS_COBRANZAS].[TIPOTRAMITE] tt
  ON pa.CODIGOTIPOTRAMITE=tt.CODIGO
  JOIN [FBS_COBRANZAS].[TIPOMATERIAJUICIO] tm
  ON pa.CODIGOTIPOMATERIAJUI=tm.CODIGO
  JOIN [FBS_GENERALES].[DIVISION] d
  ON pm.SECUENCIALOFICINA=d.SECUENCIAL
  JOIN [FBS_SEGURIDADES].[USUARIO] u
  ON pm.CODIGOUSUARIOOFICIAL=u.CODIGO
  LEFT JOIN [FBS_CARTERA].[COMENTARIO] c
  ON pm.SECUENCIAL=c.SECUENCIALPRESTAMO
  JOIN [FBS_CARTERA].[PRESTAMO_INFORMACIONADICIONAL] pi
  ON pm.SECUENCIAL = pi.SECUENCIALPRESTAMO
  JOIN [FBS_COBRANZAS].[TIPOJUDICATURA] tj
  ON pa.CODIGOTIPOJUDICATURA=tj.CODIGO
  LEFT JOIN [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] pt
  ON pm.SECUENCIAL=pt.SECUENCIALPRESTAMO
  JOIN [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ej
  ON pa.CODIGOTIPOJUDICATURA=ej.CODIGO
  WHERE 
    pm.NUMEROPRESTAMO ='" + numPretamoVar + @"'
    AND ab.SECUENCIALEMPRESA = pm.SECUENCIALEMPRESA
    AND ao.SECUENCIALOFICINA = pm.SECUENCIALOFICINA";

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

                // Tu código para conectar a la base de datos y ejecutar la consulta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Verificar si hay filas en el resultado
                    {
                        abogado = reader["ABOGADO"].ToString();
                        ListItem selectedAbogado = inlineAbogado.Items.FindByText(abogado);
                        if (selectedAbogado != null)
                        {
                            // Limpiar cualquier selección previa
                            foreach (ListItem item in inlineAbogado.Items)
                            {
                                item.Selected = false;
                            }

                            // Establecer la nueva selección
                            selectedAbogado.Selected = true;
                        }
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


                // Llamar al método Consultar() y pasar el valor necesario (si es necesario)
                oficial.Value = oficialV;
                oficina.Value = oficinaV;
                adjudicado.Value = adjudicadoV;
                proxVencimiento.Value = proVencimientoV;
                transferido.Value = saldoTransferidoV;
                descripcion.Value = descripcionV;
                txtComentario.Value = comentarioV;
                fechaIngreso.Value = fechaMaquinaV;
                fechaSistema.Value = fechaSistemaV;

                

                divTramitePrestamo.Visible = true;
                numPretamo.Value = numPretamoVar;
                tipo.Value = tipoVar;
                deudaInicial.Value = deudaInicialVar;
                saldoActual.Value = saldoVar;
            }
        }
    }
}