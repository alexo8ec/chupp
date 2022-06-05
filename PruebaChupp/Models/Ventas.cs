using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaChupp.Models
{
    [Table("ventas", Schema = "dbo")]
    public class Ventas
    {
        [Key]
        public int id_venta { get; set; }
        public int id_seguro_venta { get; set; }
        public int id_persona_venta { get; set; }
        public DateTime fecha_creacion_venta { get; set; }
        public int id_rango_venta { get; set; }
        public decimal valor_venta { get; set; }
        public int id_porcentaje_venta { get; set; }
        public decimal prima { get; set; }
    }
}
