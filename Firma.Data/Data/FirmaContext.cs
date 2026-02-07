using Microsoft.EntityFrameworkCore;

namespace Firma.Data.Data
{
    public class FirmaContext : DbContext
    {
        public FirmaContext(DbContextOptions<FirmaContext> options)
            : base(options)
        {
        }

        public DbSet<Towar> Towar { get; set; }
        public DbSet<Rodzaj> Rodzaj { get; set; }
        public DbSet<Strona> Strona { get; set; }
        public DbSet<Aktualnosc> Aktualnosc { get; set; }
        public DbSet<ElementKoszyka> ElementKoszyka { get; set; }
        public DbSet<Zamowienie> Zamowienie { get; set; }
        public DbSet<PozycjaZamowienia> PozycjaZamowienia { get; set; }

        public DbSet<Tag> Tag { get; set; }
        public DbSet<TowarTag> TowarTag { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TowarTag>()
                .HasKey(tt => new { tt.IdTowaru, tt.IdTagu });

            modelBuilder.Entity<TowarTag>()
                .HasOne(tt => tt.Towar)
                .WithMany(t => t.TowarTagi)
                .HasForeignKey(tt => tt.IdTowaru);

            modelBuilder.Entity<TowarTag>()
                .HasOne(tt => tt.Tag)
                .WithMany(t => t.TowarTagi)
                .HasForeignKey(tt => tt.IdTagu);
        }
    }
}