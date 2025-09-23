
using DocumentHelper.Model;
using DocumentHelper.Model.Document;
using DocumentHelper.Model.Reference;
using Microsoft.EntityFrameworkCore;

namespace DocumentHelper.Infrastructure.Database
{
    public class AppDBContext : DbContext
    {
        public DbSet<Unit> Units { get; set; } = null!;

        public DbSet<Document> Documents { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Section> Sections { get; set; } = null!;

        public DbSet<Item> Items { get; set; } = null!;

        public AppDBContext()
        {
            Database.EnsureCreated();
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.OwnsOne(e => e.Address, address =>
                {
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.House).HasColumnName("House");
                });

                entity.OwnsOne(e => e.DatePeriod, period =>
                {
                    period.Property(a => a.DateStart).HasColumnName("DateStart");
                    period.Property(a => a.DateEnd).HasColumnName("DateEnd");
                });
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.OwnsOne(e => e.DatePeriod, period =>
                {
                    period.Property(a => a.DateStart).HasColumnName("DateStart");
                    period.Property(a => a.DateEnd).HasColumnName("DateEnd");
                });
            });
        }

    }
}
