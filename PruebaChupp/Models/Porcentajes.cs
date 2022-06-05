using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaChupp.Models
{
    [Table("porcentajes", Schema = "dbo")]
    public class Porcentajes
    {
        [Key]
        public int id_porcentaje { get; set; }
        public string porcentaje { get; set; }
    }
}
