using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
    }
}
