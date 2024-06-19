using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            SELECT AB.NOMBRE, 
                   COUNT(AB.NOMBRE) AS [# CASOS]
            FROM [FBS_COBRANZAS].[PRESTAMOABOGADO] PA
            INNER JOIN [FBS_CARTERA].[PRESTAMOMAESTRO] PM ON PA.SECUENCIALPRESTAMO=PM.SECUENCIAL
            INNER JOIN [FBS_COBRANZAS].[ABOGADO] AB ON PA.CODIGOABOGADO=AB.CODIGO
            WHERE PM.CODIGOESTADOPRESTAMO='J' AND PA.ESTAACTIVO='1'
            GROUP BY AB.NOMBRE
            ORDER BY [# CASOS]";
                        
            // Establecer conexión y ejecutar la consulta
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // Asignar datos a la GridView
                gvCasosAbogado.DataSource = dataTable;
                gvCasosAbogado.DataBind();
            }

            litPrestamoJudicial.Text = ObtenerNumeroPrestamoJudicial();
        }

        private string ObtenerNumeroPrestamoJudicial()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
            string query = @"
            select COUNT(*) from [FBS_CARTERA].[PRESTAMOMAESTRO] 
            WHERE CODIGOESTADOPRESTAMO='J'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
                                        WHERE P.CODIGOESTADOPRESTAMO='J') AS [PORCENTAJE]
                FROM 
                    [FBS_CARTERA].[PRESTAMOMAESTRO] P
                LEFT JOIN 
                    [FBS_COBRANZAS].[PRESTAMODEMANDAJUDICIALTRAMITE] PT ON PT.SECUENCIALPRESTAMO = P.SECUENCIAL
                LEFT JOIN 
                    [FBS_COBRANZAS].[ESTADOTRAMITEDEMANDAJUDICIAL] ET ON ET.CODIGO = PT.CODIGOESTADOTRAMITEDEMJUD
                WHERE 
                    P.CODIGOESTADOPRESTAMO='J'
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