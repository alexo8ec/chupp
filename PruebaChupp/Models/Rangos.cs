using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaChupp.Models
{
    [Table("rangos", Schema = "dbo")]
    public class Rangos
    {
        [Key]
        public int id_rango { get; set; }
        public string rango { get; set; }
        public decimal porcentaje { get; set; }
    }
}
