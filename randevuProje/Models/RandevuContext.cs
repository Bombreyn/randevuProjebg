using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace randevuProje.Models
{
    public partial class RandevuContext : DbContext
    {
        public RandevuContext()
            : base("name=RandevuContext")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Kurallar> Kurallar { get; set; }
        public virtual DbSet<Odalar> Odalar { get; set; }
        public virtual DbSet<ozellik1> ozellik1 { get; set; }
        public virtual DbSet<ozellik2> ozellik2 { get; set; }
        public virtual DbSet<ozellik3> ozellik3 { get; set; }
        public virtual DbSet<Randevu> Randevu { get; set; }
        public virtual DbSet<Resim> Resim { get; set; }
        public virtual DbSet<Siteayar> Siteayar { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kurallar>()
                .Property(e => e.kural)
                .IsUnicode(false);

            modelBuilder.Entity<Odalar>()
                .Property(e => e.aciklama)
                .IsUnicode(false);

            modelBuilder.Entity<Odalar>()
                .Property(e => e.fiyat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Odalar>()
                .Property(e => e.kapakresim)
                .IsUnicode(false);

            modelBuilder.Entity<Randevu>()
                .Property(e => e.fiyat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Resim>()
                .Property(e => e.resim1)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e.slider_aciklama)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e.slider_resim)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e.harita)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e._abstract)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Siteayar>()
                .Property(e => e.keywords)
                .IsUnicode(false);
        }
    }
}
