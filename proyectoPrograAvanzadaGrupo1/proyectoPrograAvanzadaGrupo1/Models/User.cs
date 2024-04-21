using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class User
    {

        [Key]
        public int user_id { get; set; }

        [Required]
        public string usuario { get; set; }

        [Required]
        public string contraseña_hash { get; set; }

        public DateTime ultimo_login { get; set; }

        [Required]
        public int estado_id { get; set; }

        public Boolean esAdmin {  get; set; }

        [NotMapped]
        public bool MantenerActivo { get; set; }

    }
}
