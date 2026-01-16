using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data
{
    public class Towar
    {
        [Key]
        public int IdTowaru { get; set; }

        [Required(ErrorMessage = "Kod jest wymagany")]
        public string Kod { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Column(TypeName = "decimal(18,2)")] // Ważne dla cen!
        public decimal Cena { get; set; }

        public string Opis { get; set; }

        public string FotoURL { get; set; } // Ścieżka do zdjęcia

        // Relacja: Towar należy do Rodzaju
        public int IdRodzaju { get; set; }
        [ForeignKey("IdRodzaju")]
        public virtual Rodzaj? Rodzaj { get; set; }
    }
}