using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MonitorJudicial.Tests
{
    public partial class FichaCliente : System.Web.UI.Page
    {        
        protected string EstadoCliente;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Asignar el estado del cliente. Esto debería venir de tu lógica de negocio.
                EstadoCliente = ObtenerEstadoCliente();
            }
        }

        private string ObtenerEstadoCliente()
        {
            // Aquí deberías implementar la lógica para obtener el estado del cliente.
            // Por ahora, lo estableceremos a 'vida' o 'fallecido' de forma estática.
            //return "vivo"; // O "fallecido"
            return "fallecido";
        }
    }
}