using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace UdemyRealWorldUnitTest.Web.Models
{
    public partial class ntraxContext : DbContext
    {
        public ntraxContext()
        {
        }

        public ntraxContext(DbContextOptions<ntraxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PnDetail> PnDetails { get; set; }
        public virtual DbSet<PnMaster> PnMasters { get; set; }
        public virtual DbSet<Product> Products { get; set; }

     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<PnDetail>(entity =>
            {
                entity.HasIndex(e => e.PnMasterId, "IX_PnDetails_PnMasterId");

                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("true");

                entity.HasOne(d => d.PnMaster)
                    .WithMany(p => p.PnDetails)
                    .HasForeignKey(d => d.PnMasterId)
                    .HasConstraintName("FK_PnMaster_PnDetails");
            });

            modelBuilder.Entity<PnMaster>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("true");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .HasColumnName("color");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Stock).HasColumnName("stock");
            });

            modelBuilder.HasSequence("elif");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
