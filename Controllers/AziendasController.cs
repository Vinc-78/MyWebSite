using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebSite.Data;
using MyWebSite.Models;

namespace MyWebSite.Controllers
{
    public class AziendasController : Controller
    {
        private readonly MyWebSiteContext _context;

        public AziendasController(MyWebSiteContext context)
        {
            _context = context;
        }

        // GET: Aziendas
        public async Task<IActionResult> Index(string? ricerca, int pg = 1 )
        {
            List<Azienda> aziendaList = await _context.Azienda.ToListAsync();

            //la parte che gestisce la searchBar

            if (!String.IsNullOrEmpty(ricerca))
            {
                aziendaList = aziendaList.Where(s =>( s.NomeAzienda!.Contains(ricerca) 
                || s.Settore!.Contains(ricerca) || s.Città!.Contains(ricerca) || s.Indirizzo!.Contains(ricerca))).ToList();
            }

            //parte per gestire l'impaginazione
            const int pageSize = 4; //quanti record per pagina
            if (pg < 1)
                pg = 1;

            int recsCount = aziendaList.Count;

            var pager = new Inpage(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize; // quanti record saltare in funzione delle pagine

            var data = aziendaList.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Inpage = pager;

            //return _context.Azienda != null ?
            //            View(aziendaList) :
            //            Problem("Entity set 'MyWebSiteContext.Azienda'  is null.");

            return _context.Azienda != null ?
                        View(data) :
                        Problem("Entity set 'MyWebSiteContext.Azienda'  is null.");
        }

        // GET: Aziendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Azienda == null)
            {
                return NotFound();
            }

            var azienda = await _context.Azienda
                .FirstOrDefaultAsync(m => m.AziendaId == id);
            if (azienda == null)
            {
                return NotFound();
            }

            return View(azienda);
        }

        // GET: Aziendas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aziendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AziendaId,NomeAzienda,Settore,Città,Indirizzo")] Azienda azienda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(azienda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //aggiungo una stringa per gestire i messaggi di errori:

            string errors = string.Join("; ", ModelState.Values
                                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage));

            ModelState.AddModelError("", errors);

            return View(azienda);
        }

        // GET: Aziendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Azienda == null)
            {
                return NotFound();
            }

            var azienda = await _context.Azienda.FindAsync(id);
            if (azienda == null)
            {
                return NotFound();
            }
            return View(azienda);
        }

        // POST: Aziendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AziendaId,NomeAzienda,Settore,Città,Indirizzo")] Azienda azienda)
        {
            if (id != azienda.AziendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(azienda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AziendaExists(azienda.AziendaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(azienda);
        }

        // GET: Aziendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Azienda == null)
            {
                return NotFound();
            }

            var azienda = await _context.Azienda
                .FirstOrDefaultAsync(m => m.AziendaId == id);
            if (azienda == null)
            {
                return NotFound();
            }

            return View(azienda);
        }

        // POST: Aziendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Azienda == null)
            {
                return Problem("Entity set 'MyWebSiteContext.Azienda'  is null.");
            }
            var azienda = await _context.Azienda.FindAsync(id);
            if (azienda != null)
            {
                _context.Azienda.Remove(azienda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AziendaExists(int id)
        {
            return (_context.Azienda?.Any(e => e.AziendaId == id)).GetValueOrDefault();
        }
    }
}
