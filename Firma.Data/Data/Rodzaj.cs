using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data
{
    public class Rodzaj
    {
        [Key]
        public int IdRodzaju { get; set; }

        [Required(ErrorMessage = "Nazwa rodzaju jest wymagana")]
        public string Nazwa { get; set; }

        public string Opis { get; set; }

        // Relacja: Jeden rodzaj ma wiele towarów
        public virtual ICollection<Towar>? Towary { get; set; }
    }
}