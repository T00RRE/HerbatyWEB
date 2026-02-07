using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using Firma.PortalWWW.Services; // Twój namespace z EmailSender

namespace Firma.PortalWWW.Controllers
{
    public class ZamowieniaController : Controller
    {
        private readonly FirmaContext _context;
        private readonly EmailSender _emailSender; // 1. Dodajemy pole

        // 2. Wstrzykujemy w konstruktorze
        public ZamowieniaController(FirmaContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Wyświetlenie formularza danych do wysyłki
        public async Task<IActionResult> Potwierdzenie()
        {
            // Pobieramy koszyk, żeby wyświetlić podsumowanie (opcjonalne, ale warto wiedzieć co się kupuje)
            var koszyk = await _context.ElementKoszyka
                .Include(e => e.Towar)
                .ToListAsync();

            if (!koszyk.Any())
            {
                return RedirectToAction("Index", "Koszyk");
            }

            ViewBag.Suma = koszyk.Sum(e => e.Ilosc * e.Towar.Cena);
            return View(new Zamowienie()); // Przekazujemy pusty model do wypełnienia
        }

        // POST: Finalizacja zamówienia (Zapis + Email)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Potwierdzenie(Zamowienie zamowienie)
        {
            // 1. Walidacja i przygotowanie danych
            zamowienie.DataZamowienia = DateTime.Now;

            // Pobierz elementy z koszyka
            var elementyKoszyka = await _context.ElementKoszyka
                .Include(e => e.Towar)
                .ToListAsync();

            if (!elementyKoszyka.Any()) return RedirectToAction("Index", "Home");

            zamowienie.Razem = elementyKoszyka.Sum(e => e.Ilosc * e.Towar.Cena);

            // 2. Zapisz zamówienie w bazie
            _context.Add(zamowienie);
            await _context.SaveChangesAsync(); // Tutaj zamowienie dostaje ID

            // 3. Przepisz pozycje z koszyka do PozycjiZamowienia
            foreach (var element in elementyKoszyka)
            {
                var pozycja = new PozycjaZamowienia
                {
                    IdZamowienia = zamowienie.IdZamowienia,
                    IdTowaru = element.IdTowaru,
                    Ilosc = element.Ilosc,
                    Cena = element.Towar.Cena
                };
                _context.Add(pozycja);
            }

            // 4. Wyczyść koszyk
            _context.ElementKoszyka.RemoveRange(elementyKoszyka);
            await _context.SaveChangesAsync();

            // ==================================================
            // 5. WYSYŁKA MAILA (Tutaj dzieje się magia)
            // ==================================================
            try
            {
                string temat = $"Potwierdzenie zamówienia nr {zamowienie.IdZamowienia}";
                string treść = "<h1>Dziękujemy za zakupy!</h1>";
                treść += $"<p>Witaj {zamowienie.Imie}, Twoje zamówienie zostało przyjęte.</p>";
                treść += "<h3>Szczegóły zamówienia:</h3><ul>";

                foreach (var item in elementyKoszyka)
                {
                    treść += $"<li>{item.Towar.Nazwa} - {item.Ilosc} szt. x {item.Towar.Cena} zł</li>";
                }
                treść += "</ul>";
                treść += $"<h4>Łącznie do zapłaty: {zamowienie.Razem} zł</h4>";
                treść += $"<p>Adres dostawy: {zamowienie.Ulica}, {zamowienie.KodPocztowy} {zamowienie.Miasto}</p>";

                // UWAGA: Upewnij się, że w klasie Zamowienie masz pole Email!
                // Jeśli nie masz, musisz je dodać w Firma.Data/Data/Zamowienie.cs
                if (!string.IsNullOrEmpty(zamowienie.Email))
                {
                    await _emailSender.WyslijEmail(zamowienie.Email, temat, treść);
                }
            }
            catch (Exception ex)
            {
                // Logujemy błąd, ale nie przerywamy procesu (klient i tak złożył zamówienie)
                Console.WriteLine($"Nie udało się wysłać maila: {ex.Message}");
            }

            return RedirectToAction("Sukces");
        }

        public IActionResult Sukces()
        {
            return View();
        }
    }
}