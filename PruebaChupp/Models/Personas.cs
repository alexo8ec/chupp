using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaChupp.Models
{
    [Table("personas", Schema = "dbo")]
    public class Personas
    {
        [Key]
        public int id_persona { get; set; }
        public string nombre_persona { get; set; }
        public string apellido_persona { get; set; }
        public string cedula_persona { get; set; }
        public string direccion_persona { get; set; }
        public string email_persona { get; set; }
        public string telefono_persona { get; set; }
        public string celular_persona { get; set; }
        public int estado_persona { get; set; }
        public DateTime fecha_nacimiento_persona { get; set; }
    }
}
