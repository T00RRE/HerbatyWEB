using Firma.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Firma.Intranet.Controllers
{
    public class ZamowienieController : Controller
    {
        private readonly FirmaContext _context;

        public ZamowienieController(FirmaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var zamowienia = _context.Zamowienie
                .OrderByDescending(z => z.DataZamowienia)
                .ToList();
            return View(zamowienia);
        }
        public IActionResult Details(int id)
        {
            var zamowienie = _context.Zamowienie
                .Include(z => z.PozycjeZamowienia)
                .ThenInclude(p => p.Towar)
                .FirstOrDefault(z => z.IdZamowienia == id);

            if (zamowienie == null) return NotFound();

            return View(zamowienie);
        }
    }
}