using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore;    
using Firma.Data.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Firma.Intranet.Controllers
{
    public class TowarController : Controller
    {
        private readonly FirmaContext _context;

        public TowarController(FirmaContext context)
        {
            _context = context;
        }

        // 1. Lista towarów
        public IActionResult Index(string szukanaFraza, int? idRodzaju)
        {
            // 1. Przygotuj zapytanie (jeszcze nie wysłane do bazy)
            var towary = _context.Towar.Include(t => t.Rodzaj).AsQueryable();

            // 2. Filtruj po nazwie (jeśli wpisano)
            if (!string.IsNullOrEmpty(szukanaFraza))
            {
                towary = towary.Where(t => t.Nazwa.Contains(szukanaFraza) || t.Kod.Contains(szukanaFraza));
                ViewBag.Fraza = szukanaFraza;
            }

            // 3. Filtruj po kategorii (jeśli wybrano)
            if (idRodzaju.HasValue)
            {
                towary = towary.Where(t => t.IdRodzaju == idRodzaju);
            }

            // 4. Załaduj listę kategorii do dropdowna
            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", idRodzaju);

            // 5. Wykonaj i zwróć wynik
            return View(towary.ToList());
        }

        // 2. Formularz dodawania
        public IActionResult Create()
        {
            ViewBag.ListaRodzajow = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa");

            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var towar = _context.Towar.Find(id);
            if (towar == null) return NotFound();

            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", towar.IdRodzaju);

            return View(towar);
        }

        // 2. POST: Zapisz zmiany
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Towar towar)
        {
            if (id != towar.IdTowaru) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(towar);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", towar.IdRodzaju);
            return View(towar);
        }
        // 3. Zapisywanie
        [HttpPost]
        public IActionResult Create(Towar towar)
        {
            if (ModelState.IsValid)
            {
                _context.Towar.Add(towar);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListaRodzajow = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa");
            return View(towar);
        }
        public IActionResult Delete(int id)
        {
            var towar = _context.Towar.Find(id);
            if (towar != null)
            {
                _context.Towar.Remove(towar);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}