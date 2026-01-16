using Microsoft.AspNetCore.Mvc;
using Firma.Data.Data;
using System.Linq;

namespace Firma.Intranet.Controllers
{
    public class AktualnoscController : Controller
    {
        private readonly FirmaContext _context;

        public AktualnoscController(FirmaContext context)
        {
            _context = context;
        }

        // 1. Lista aktualności
        public IActionResult Index()
        {
            return View(_context.Aktualnosc.OrderBy(a => a.Pozycja).ToList());
        }

        // 2. Formularz dodawania
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
        // 3. Zapisywanie
        [HttpPost]
        public IActionResult Create(Aktualnosc aktualnosc)
        {
            if (ModelState.IsValid)
            {
                _context.Aktualnosc.Add(aktualnosc);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aktualnosc);
        }

        // 4. Usuwanie 
        public IActionResult Delete(int id)
        {
            var news = _context.Aktualnosc.Find(id);
            if (news != null)
            {
                _context.Aktualnosc.Remove(news);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}