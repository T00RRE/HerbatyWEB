using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Firma.Data.Data
{
    public class Tag
    {
        [Key]
        public int IdTagu { get; set; }

        [Required(ErrorMessage = "Nazwa tagu jest wymagana")]
        public string Nazwa { get; set; }

        public virtual ICollection<TowarTag> TowarTagi { get; set; }
    }
}