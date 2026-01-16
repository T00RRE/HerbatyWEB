using Firma.Data.Data;
using Firma.PortalWWW.Models; // Zostawiamy dla ErrorViewModel
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Ważne dla funkcji Include i ToList
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirmaContext _context;

        // Wstrzykujemy bazę danych w konstruktorze
        public HomeController(FirmaContext context)
        {
            _context = context;
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(string szukanaFraza, int? idRodzaju)
        {
            // ViewBag
            var kategorie = await _context.Rodzaj.ToListAsync();
            ViewBag.Rodzaje = new SelectList(kategorie, "IdRodzaju", "Nazwa", idRodzaju);
            ViewBag.Aktualnosci = await _context.Aktualnosc.OrderBy(a => a.Pozycja).ToListAsync();
            // AsQueryable 
            var zapytanie = _context.Towar
                .Include(t => t.Rodzaj)
                .AsQueryable();

            if (!string.IsNullOrEmpty(szukanaFraza))
            {
                zapytanie = zapytanie.Where(t => t.Nazwa.Contains(szukanaFraza) || t.Opis.Contains(szukanaFraza));
                ViewBag.Fraza = szukanaFraza;
            }

            if (idRodzaju.HasValue)
            {
                zapytanie = zapytanie.Where(t => t.IdRodzaju == idRodzaju);
            }

            var towary = await zapytanie.OrderByDescending(t => t.IdTowaru).ToListAsync();

            return View(towary);
        }
    }
}