using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Firma.Data.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Firma.Intranet.Controllers
{
    public class TowarController : Controller
    {
        private readonly FirmaContext _context;

        public TowarController(FirmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string szukanaFraza, int? idRodzaju)
        {
            var towary = _context.Towar
                .Include(t => t.Rodzaj)
                .Include(t => t.TowarTagi)
                    .ThenInclude(tt => tt.Tag)
                .AsQueryable();

            if (!string.IsNullOrEmpty(szukanaFraza))
            {
                towary = towary.Where(t => t.Nazwa.Contains(szukanaFraza) || t.Kod.Contains(szukanaFraza));
                ViewBag.Fraza = szukanaFraza;
            }

            if (idRodzaju.HasValue)
            {
                towary = towary.Where(t => t.IdRodzaju == idRodzaju);
            }

            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", idRodzaju);

            return View(await towary.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa");
            ViewBag.WszystkieTagi = await _context.Tag.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTowaru,Kod,Nazwa,Cena,Opis,FotoURL,IdRodzaju")] Towar towar, int[] wybraneTagi)
        {
            ModelState.Remove("TowarTagi");

            if (ModelState.IsValid)
            {
                _context.Add(towar);
                await _context.SaveChangesAsync(); 

                if (wybraneTagi != null)
                {
                    foreach (var tagId in wybraneTagi)
                    {
                        var nowePowiazanie = new TowarTag
                        {
                            IdTowaru = towar.IdTowaru,
                            IdTagu = tagId
                        };
                        _context.Add(nowePowiazanie);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", towar.IdRodzaju);
            ViewBag.WszystkieTagi = await _context.Tag.ToListAsync();
            return View(towar);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var towar = await _context.Towar
                .Include(t => t.TowarTagi)
                .FirstOrDefaultAsync(t => t.IdTowaru == id);

            if (towar == null) return NotFound();

            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", towar.IdRodzaju);
            ViewBag.WszystkieTagi = await _context.Tag.ToListAsync();
            return View(towar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTowaru,Kod,Nazwa,Cena,Opis,FotoURL,IdRodzaju")] Towar towar, int[] wybraneTagi)
        {
            if (id != towar.IdTowaru) return NotFound();

            ModelState.Remove("TowarTagi");

            if (ModelState.IsValid)
            {
                try
                {
                    var towarWBazie = await _context.Towar
                        .Include(t => t.TowarTagi)
                        .FirstOrDefaultAsync(t => t.IdTowaru == id);

                    if (towarWBazie == null) return NotFound();
                    towarWBazie.Kod = towar.Kod;
                    towarWBazie.Nazwa = towar.Nazwa;
                    towarWBazie.Cena = towar.Cena;
                    towarWBazie.Opis = towar.Opis;
                    towarWBazie.FotoURL = towar.FotoURL;
                    towarWBazie.IdRodzaju = towar.IdRodzaju;
                    towarWBazie.TowarTagi.Clear();

                    if (wybraneTagi != null)
                    {
                        foreach (var tagId in wybraneTagi)
                        {
                            towarWBazie.TowarTagi.Add(new TowarTag { IdTagu = tagId, IdTowaru = id });
                        }
                    }

                    _context.Update(towarWBazie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Towar.Any(e => e.IdTowaru == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Rodzaje = new SelectList(_context.Rodzaj, "IdRodzaju", "Nazwa", towar.IdRodzaju);
            ViewBag.WszystkieTagi = await _context.Tag.ToListAsync();
            return View(towar);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var towar = await _context.Towar.FindAsync(id);
            if (towar != null)
            {
                _context.Towar.Remove(towar);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}