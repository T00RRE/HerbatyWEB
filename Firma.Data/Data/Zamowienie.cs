using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firma.Data.Data
{
    public class Zamowienie
    {
        [Key]
        public int IdZamowienia { get; set; }

        public DateTime DataZamowienia { get; set; }

        // Dane adresowe
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Ulica i numer są wymagane")]
        public string Ulica { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane")]
        public string Miasto { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        public string KodPocztowy { get; set; }

        public string Telefon { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Razem { get; set; }

        public virtual ICollection<PozycjaZamowienia> PozycjeZamowienia { get; set; }
    }
}