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
    public class InterventoesController : Controller
    {
        private readonly MyWebSiteContext _context;

        public InterventoesController(MyWebSiteContext context)
        {
            _context = context;
        }

        // GET: Interventoes
        public async Task<IActionResult> Index()
        {
            var myWebSiteContext = _context.Intervento.Include(i => i.Azienda);
            return View(await myWebSiteContext.ToListAsync());
        }

        // GET: Interventoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Intervento == null)
            {
                return NotFound();
            }

            var intervento = await _context.Intervento
                .Include(i => i.Azienda)
                .FirstOrDefaultAsync(m => m.InterventoId == id);
            if (intervento == null)
            {
                return NotFound();
            }

            return View(intervento);
        }

        // GET: Interventoes/Create
        public IActionResult Create()
        {
            ViewData["AziendaNome"] = new SelectList(_context.Azienda, "NomeAzienda", "NomeAzienda");
            return View();

            //ViewData["AziendaID"] = new SelectList(_context.Azienda, "AziendaId", "AziendaId");
            //return View();
        }

        // POST: Interventoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Intervento intervento,  Azienda azienda)
        {
            var specificAzienda = _context.Azienda.Where(m => m.NomeAzienda == azienda.NomeAzienda).FirstOrDefault();

            intervento.AziendaID = specificAzienda.AziendaId;

            intervento.Completato = false;

            Intervento NewIntervento = new Intervento()
            {
                TipoIntervento = intervento.TipoIntervento,
                DataIntervento = intervento.DataIntervento,
                Completato = intervento.Completato,
                AziendaID = intervento.AziendaID
  
            };

            _context.Add(NewIntervento);
              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));

            //if (ModelState.IsValid)
            //{


            //    _context.Add(intervento);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            //ViewData["AziendaNome"] = new SelectList(_context.Azienda, "NomeAzienda", "NomeAzienda", azienda.NomeAzienda);
            ////ViewData["AziendaID"] = new SelectList(_context.Azienda, "AziendaId", "AziendaId", intervento.AziendaID);
            //return View(intervento);
        }

        // GET: Interventoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Intervento == null)
            {
                return NotFound();
            }

            var intervento = await _context.Intervento.FindAsync(id);
            if (intervento == null)
            {
                return NotFound();
            }

            var Azienda = _context.Azienda.Where(m => m.AziendaId == intervento.AziendaID).FirstOrDefault();

            //mi serve anche l'id dell'azienda in modo che il form della view passa AziendaID(campo hidden) 

            //lo prendo da intervento.AziendaId 


            // qui passo il nome della azienda alla view e lo utilizzo solo li
            ViewData["AziendaNome"] = Azienda.NomeAzienda;
            //ViewData["AziendaID"] = new SelectList(_context.Azienda, "AziendaId", "AziendaId", intervento.AziendaID);
            return View(intervento);
        }

        // POST: Interventoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, bool esito, Intervento intervento)
        {
            if (id != intervento.InterventoId)
            {
                return NotFound();
            }

            
                try
                {
                    if (esito = true) intervento.Completato = true;
                    else intervento.Completato = false;

                    

                    _context.Update(intervento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InterventoExists(intervento.InterventoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            //ViewData["AziendaID"] = new SelectList(_context.Azienda, "AziendaId", "AziendaId", intervento.AziendaID);
            return View(intervento);
        }

        // GET: Interventoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Intervento == null)
            {
                return NotFound();
            }

            var intervento = await _context.Intervento
                .Include(i => i.Azienda)
                .FirstOrDefaultAsync(m => m.InterventoId == id);
            if (intervento == null)
            {
                return NotFound();
            }

            return View(intervento);
        }

        // POST: Interventoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Intervento == null)
            {
                return Problem("Entity set 'MyWebSiteContext.Intervento'  is null.");
            }
            var intervento = await _context.Intervento.FindAsync(id);
            if (intervento != null)
            {
                _context.Intervento.Remove(intervento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InterventoExists(int id)
        {
            return (_context.Intervento?.Any(e => e.InterventoId == id)).GetValueOrDefault();
        }
    }
}
