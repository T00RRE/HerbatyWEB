using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data
{
    public class Strona
    {
        [Key]
        public int IdStrony { get; set; }
        [Required]
        public string TytulOdnosnika { get; set; }
        [Required]
        public string TytulTresci { get; set; }
        public string Tresc { get; set; }
        public int Pozycja { get; set; }
    }

    public class Aktualnosc
    {
        [Key]
        public int IdAktualnosci { get; set; }
        [Required]
        public string TytulOdnosnika { get; set; }
        [Required]
        public string TytulTresci { get; set; }
        public string Tresc { get; set; }
        public int Pozycja { get; set; }
    }
}