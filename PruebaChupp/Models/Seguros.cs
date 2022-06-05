using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaChupp.Models
{
    [Table("seguros", Schema = "dbo")]
    public class Seguros
    {
        [Key]
        public int id_seguro { get; set; }
        public string nombre_seguro { get; set; }
        public string codigo_seguro { get; set; }
        public DateTime fecha_creacion_seguro { get; set; }
        public DateTime fecha_modificacion_seguro { get; set; }
        public string rango_edad_seguro { get; set; }
        public string porcentaje_seguro { get; set; }
        public int id_tipo_seguro { get; set; }
        public decimal valor_seguro { get; set; }
        public int estado_seguro { get; set; }
    }
}
