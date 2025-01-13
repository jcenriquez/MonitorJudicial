using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonitorJudicial.Models
{
    [Table("EstadoPrestamosConPorcentaje_Vista", Schema = "FBS_COBRANZAS")]
    public class EstadoPrestamosConPorcentaje
    {
        [Key]
        public string NOMBRE { get; set; }

        public int PREJUDICIAL { get; set; }

        public int JUDICIAL { get; set; }

        [Column("JUDICIAL CON ACUERDO AL DÍA")]
        public int JUDICIAL_CON_ACUERDO_AL_DIA { get; set; }

        [Column("JUDICIAL CON ACUERDO VENCIDO")]
        public int JUDICIAL_CON_ACUERDO_VENCIDO { get; set; }

        public int CASTIGADO { get; set; }

        public int TOTAL { get; set; }

        public double PORCENTAJE { get; set; }
    }

    public class Prestamo
    {
        [Key]
        public string NUMEROPRESTAMO { get; set; }
        public string AGENCIA { get; set; }
        public string NOMBRE_SOCIO { get; set; }
        public string IDENTIDAD { get; set; }
        public string NUM_JUICIO { get; set; }
        public string FECHA_ADJUDICACION { get; set; }
        public string FECHA_INICIO_DEMANDA { get; set; }
        public decimal DEUDAINICIAL { get; set; }
        public decimal SALDO_ACTUAL { get; set; }
        public decimal SALDO_TRANSFERIDO { get; set; }
        public string NUM_CLIENTE { get; set; }
        public string NOMBRE_ABOGADO { get; set; }
        public string ESTADO_JUDICIAL { get; set; }
        public string GARANTIA { get; set; }
        public string JUZGADO { get; set; }
        public string TRAMITE_JUDICIAL { get; set; }
    }
}