using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class KoszykController : Controller
    {
        private readonly FirmaContext _context;

        public KoszykController(FirmaContext context)
        {
            _context = context;
        }

        // Akcja: Wyświetl zawartość koszyka
        public async Task<IActionResult> Index()
        {
            var idSesji = HttpContext.Session.Id; // Pobierz identyfikator użytkownika

            var elementy = await _context.ElementKoszyka
                .Where(k => k.IdSesjiKoszyka == idSesji)
                .Include(k => k.Towar) // Pobierz też dane o towarze (nazwa, cena)
                .ToListAsync();

            return View(elementy);
        }

        // Akcja: Dodaj do koszyka
        public async Task<IActionResult> DodajDoKoszyka(int id)
        {
            // --- POPRAWKA START ---
            // Musimy coś zapisać w sesji, żeby ID się nie zmieniało przy przeładowaniu strony
            if (HttpContext.Session.GetString("UserSession") == null)
            {
                HttpContext.Session.SetString("UserSession", "Active");
            }
            // --- POPRAWKA STOP ---

            var idSesji = HttpContext.Session.Id;

            // Reszta kodu bez zmian...
            var element = await _context.ElementKoszyka
                .FirstOrDefaultAsync(k => k.IdSesjiKoszyka == idSesji && k.IdTowaru == id);

            if (element != null)
            {
                element.Ilosc++;
            }
            else
            {
                element = new ElementKoszyka
                {
                    IdSesjiKoszyka = idSesji,
                    IdTowaru = id,
                    Ilosc = 1,
                    DataUtworzenia = DateTime.Now
                };
                _context.ElementKoszyka.Add(element);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}