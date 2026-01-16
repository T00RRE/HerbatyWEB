using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Firma.PortalWWW.Controllers
{
    public class ZamowienieController : Controller
    {
        private readonly FirmaContext _context;

        public ZamowienieController(FirmaContext context)
        {
            _context = context;
        }

        // 1. (GET)
        public IActionResult Potwierdzenie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Potwierdzenie(Zamowienie zamowienie)
        {
            var idSesji = HttpContext.Session.Id;
            var elementyKoszyka = await _context.ElementKoszyka
                .Where(k => k.IdSesjiKoszyka == idSesji)
                .Include(k => k.Towar)
                .ToListAsync();

            if (!elementyKoszyka.Any())
            {
                ModelState.AddModelError("", "Twój koszyk jest pusty!");
                return View(zamowienie);
            }

            zamowienie.DataZamowienia = DateTime.Now;
            zamowienie.Razem = elementyKoszyka.Sum(e => e.Ilosc * e.Towar.Cena);

            _context.Zamowienie.Add(zamowienie);
            await _context.SaveChangesAsync(); 

            foreach (var element in elementyKoszyka)
            {
                var pozycja = new PozycjaZamowienia
                {
                    IdZamowienia = zamowienie.IdZamowienia,
                    IdTowaru = element.IdTowaru,
                    Ilosc = element.Ilosc,
                    Cena = element.Towar.Cena
                };
                _context.PozycjaZamowienia.Add(pozycja);
            }

            // Wyczyść koszyk
            _context.ElementKoszyka.RemoveRange(elementyKoszyka);
            await _context.SaveChangesAsync();

            return RedirectToAction("Sukces");
        }

        public IActionResult Sukces()
        {
            return View();
        }
    }
}