using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class Carrito
    {
        [Key]
        public int CarritoId { get; set; }

        [ForeignKey("Usuarios")]
        public int UserId { get; set; }
        public User Usuario { get; set; }

        [ForeignKey("Productos")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
    }
}
