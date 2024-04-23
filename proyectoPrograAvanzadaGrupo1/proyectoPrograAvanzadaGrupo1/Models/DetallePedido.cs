using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class DetallePedido
    {
        [Key]
        public int detalle_id { get; set; }

        public int orden_id { get; set; }

        public int product_id { get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Precio { get; set; }

        // Relación con la tabla Pedido
        [ForeignKey("orden_id")]
        public Pedido Pedido { get; set; }

        // Relación con la tabla Producto
        [ForeignKey("product_id")]
        public Producto Producto { get; set; }
    }
}
