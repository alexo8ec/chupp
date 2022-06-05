using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaChupp.Models
{
    [Table("tipo_seguros", Schema = "dbo")]
    public class TipoSeguros
    {
        [Key]
        public int id_tipo_seguro { get; set; }
        public string tipo_seguro { get; set; }
        public int estado_seguro { get; set; }
    }
}
