global using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebAPIwithEFCore.Models;

namespace WebAPIwithEFCore
{
    public class StudentContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;
                                          Database=StudentContext;
                                          Trusted_Connection=True;
                                          TrustServerCertificate=True;");

            optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.ConfigureWarnings(warnings =>
            {
                warnings.Throw(CoreEventId.LazyLoadOnDisposedContextWarning); // ném ra ngoại lệ
            });
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Class>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Class)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
