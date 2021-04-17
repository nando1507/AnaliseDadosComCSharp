using System;
using AnaliseDadosDotNetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnaliseDadosDotNetCore.DAL.Context
{
    public partial class DbApiContext : DbContext
    {
        public DbApiContext()
        {
        }

        public DbApiContext(DbContextOptions<DbApiContext> options) : base(options)
        {
        }

        public virtual DbSet<TbCoronaVirus> tbCoronaVirus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Program.sqlConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Define o Schema do banco (Melhor utilizado em oracle)
            modelBuilder.HasDefaultSchema("dbo");
            
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TbCoronaVirus>(entity =>
             {
                 entity.HasKey(e => e.IdIndex)
                     .HasName("PK__TbCorona__F2DE05CD66E4BD47");

                 entity.ToTable("TbCoronaVirus");

                 entity.Property(e => e.AdminRegion1)
                     .HasMaxLength(100)
                     .IsUnicode(false);

                 entity.Property(e => e.AdminRegion2)
                     .HasMaxLength(100)
                     .IsUnicode(false);

                 entity.Property(e => e.CountryRegion)
                     .HasMaxLength(100)
                     .IsUnicode(false)
                     .HasColumnName("Country_Region");

                 entity.Property(e => e.Id)
                    .HasColumnName("ID");

                 entity.Property(e => e.Iso2)
                     .HasMaxLength(100)
                     .IsUnicode(false)
                     .HasColumnName("ISO2");

                 entity.Property(e => e.Iso3)
                     .HasMaxLength(100)
                     .IsUnicode(false)
                     .HasColumnName("ISO3");

                 entity.Property(e => e.Latitude).HasColumnType("numeric(25, 15)");

                 entity.Property(e => e.Longitude).HasColumnType("numeric(25, 15)");

                 entity.Property(e => e.Updated).HasColumnType("date");
             });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
