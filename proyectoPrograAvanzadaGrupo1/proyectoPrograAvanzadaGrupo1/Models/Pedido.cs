using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class Pedido
    {
        [Key]
        public int orden_id { get; set; }

        public int user_id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime fecha_pedido { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal total { get; set; }

        // Relación con la tabla Usuario (User)
        [ForeignKey("user_id")]
        public User User { get; set; }

        // Colección de detalles de pedido
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
