using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data
{
    public class PozycjaZamowienia
    {
        [Key]
        public int IdPozycjiZamowienia { get; set; }

        public int IdZamowienia { get; set; }
        public int IdTowaru { get; set; }
        public int Ilosc { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cena { get; set; } // Cena w momencie zakupu (bo cena towaru może się zmienić!)

        [ForeignKey("IdZamowienia")]
        public virtual Zamowienie Zamowienie { get; set; }

        [ForeignKey("IdTowaru")]
        public virtual Towar Towar { get; set; }
    }
}