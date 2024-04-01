using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace proyectoPrograAvanzadaGrupo1.Models
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-3P5MFNR;Database=PrograAvanzadaGrupo1;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        public DbSet<User> Usuarios { get; set; }
    }
}
