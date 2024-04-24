using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    [Table("Detalles_de_pedidos")]
    public class DetallePedido
    {
        [Key]
        [Column("detalle_id")]
        public int DetalleId { get; set; }

        [Column("orden_id")]
        public int OrdenId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [ForeignKey("OrdenId")]
        public Pedido Pedido { get; set; }

        [ForeignKey("ProductId")]
        public Producto Producto { get; set; }
    }
}
