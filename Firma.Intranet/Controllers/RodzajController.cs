using Microsoft.AspNetCore.Mvc;
using Firma.Data.Data;
using System.Linq;

namespace Firma.Intranet.Controllers
{
    public class RodzajController : Controller
    {
        private readonly FirmaContext _context;

        public RodzajController(FirmaContext context)
        {
            _context = context;
        }

        // 1. Widok listy (Tabela z rodzajami)
        public IActionResult Index()
        {
            var rodzaje = _context.Rodzaj.ToList(); // Pobierz wszystko z bazy
            return View(rodzaje);
        }

        // 2. Widok dodawania (Formularz)
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // 404
            }

            var aktualnosc = _context.Aktualnosc.Find(id);

            if (aktualnosc == null)
            {
                return NotFound(); // 404
            }

            return View(aktualnosc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Aktualnosc aktualnosc)
        {
            if (id != aktualnosc.IdAktualnosci)
            {
                return NotFound(); //404
            }

            if (ModelState.IsValid)
            {
                _context.Update(aktualnosc);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(aktualnosc);
        }
        // 3. Akcja zapisu
        [HttpPost]
        public IActionResult Create(Rodzaj rodzaj)
        {
            if (ModelState.IsValid)
            {
                _context.Rodzaj.Add(rodzaj); 
                _context.SaveChanges();      
                return RedirectToAction("Index"); 
            }
            return View(rodzaj);
        }
        public IActionResult Delete(int id)
        {
            var rodzaj = _context.Rodzaj.Find(id);
            if (rodzaj != null)
            {
                _context.Rodzaj.Remove(rodzaj);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}