namespace MiAppCRUD.Server.Data
{
    using Microsoft.EntityFrameworkCore;
    using MiAppCRUD.Server.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
