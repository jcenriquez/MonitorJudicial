using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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

                // Llamar al método Consultar() y pasar el valor necesario (si es necesario)
                divTramitePrestamo.Visible = true;
                numPretamo.Value = numPretamoVar;
                tipo.Value = tipoVar;
                deudaInicial.Value = deudaInicialVar;
                saldoActual.Value = saldoVar;
            }
        }
    }
}