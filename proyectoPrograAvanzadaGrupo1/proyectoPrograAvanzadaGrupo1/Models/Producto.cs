using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class Producto
    {

        [Key]
        public int producto_id { get; set; }
        [Required]
        public string nombre_producto { get; set; }

        [Required]
        public decimal precio { get; set; }

        public DateTime fecha_salida { get; set; }
        
        public int cantidad {  get; set; }

        public string video_url { get; set; }

        public int estado_id { get; set; }

        public string descripcion { get; set; }

        public string? foto { get; set; }

        public decimal precio_descuento { get; set; }

        [Required]
        public Boolean en_descuento { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }



    }
}
