using System;
using System.Collections.Generic;
using System.Data;
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
            if (!IsPostBack)
            {
                // Llama al método para llenar el GridView solo si no es una solicitud de postback
                //LlenarGridView();
            }
        }

        protected void LlenarGridView()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Número"), new DataColumn("Tipo"), new DataColumn("Deuda Inicial"), new DataColumn("Saldo"), new DataColumn("Adjudicado"), new DataColumn("Vencimiento"), new DataColumn("Estado"), });
            dt.Rows.Add("0136012922", "REACTIVESE", "20000.00", "19600.00", "27/04/2023", "27/04/2030", "JUDICIAL");

            GridView1.DataSource = dt;
            GridView1.DataBind();

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //ProbarLinq();
            LlenarGridView();
        }
       

    }
}