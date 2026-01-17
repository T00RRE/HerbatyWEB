using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data
{
    public class ElementKoszyka
    {
        [Key]
        public int IdElementuKoszyka { get; set; }

        public string IdSesjiKoszyka { get; set; } 

        public DateTime DataUtworzenia { get; set; }

        public int IdTowaru { get; set; }

        [ForeignKey("IdTowaru")]
        public virtual Towar Towar { get; set; }

        public int Ilosc { get; set; }
    }
}