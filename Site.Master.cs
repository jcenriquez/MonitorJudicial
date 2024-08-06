using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MonitorJudicial
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Permisos();
        }
        public void Permisos()
        {
            if (Session["Nombres"] != null)
            {
                lblNombres.Text = Session["Nombres"].ToString();
            }
            else
            {
                lblNombres.Text = "Invitado"; // Valor predeterminado si no hay información en la sesión
            }
            string rol = (string)(Session["Rol"]);
            if (rol == "1")
            {
                admin1.Visible = true;
                admin2.Visible = true;
                //Response.Redirect("Default.aspx");
            }
            else
            {
                admin1.Visible = false;
                admin2.Visible = false;
                //Response.Redirect("~/Views/ConsultaPorCliente.aspx");
            }
        }
    }
}