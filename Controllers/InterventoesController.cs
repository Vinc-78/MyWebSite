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
        public async Task<IActionResult> Index(string? InterventoType, 
            string? InterventoDate, bool? InterventoState, string? InterventoAzienda, string sortOrder)
        {
            //gestione ordinamento per colonna
            //cliccando sul titolo si cambia l'ordine ( il sortOrder parameter )
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["NameSortParm"] = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewData["StateSortParm"] = sortOrder == "Do" ? "Not_do" : "Do";
            ViewData["AziendaSortParm"] = sortOrder == "Azienda_asc" ? "Azienda_desc" : "Azienda_asc";

            
            var interventi = from m in _context.Intervento.Include(i => i.Azienda)
                             select m;

            switch (sortOrder)
            {
                case "date_desc":
                    interventi = interventi.OrderByDescending(s => s.DataIntervento);
                    break;
                case "name_asc":
                    interventi = interventi.OrderBy(s => s.TipoIntervento);
                    break;
                case "name_desc":
                    interventi = interventi.OrderByDescending(s => s.TipoIntervento);
                    break;
                case "Do":
                    interventi = interventi.OrderBy(s => s.Completato);
                    break;
                case "Not_do":
                    interventi = interventi.OrderByDescending(s => s.Completato);
                    break;
                case "Azienda_asc":
                    interventi = interventi.OrderBy(s => s.Azienda.NomeAzienda);
                    break;
                case "Azienda_desc":
                    interventi = interventi.OrderByDescending(s => s.Azienda.NomeAzienda);
                    break;
                default:
                    interventi = interventi.OrderBy(s => s.DataIntervento);
                    break;
            }

            //gestione filtri
            // per ricavare gli elementi da mettere nel select
            IQueryable<string> tipoQuery = from m in _context.Intervento
                                           orderby m.TipoIntervento
                                           select m.TipoIntervento;

            IQueryable<string> dataQuery = from m in _context.Intervento
                                           orderby m.DataIntervento
                                           select m.DataIntervento;

            IQueryable<string> AziendaQuery = from m in _context.Intervento.Include(i => i.Azienda)
                                            orderby m.Azienda.NomeAzienda
                                            select m.Azienda.NomeAzienda;
           // Nota:
           //Il filtro è gestito a parte dall'ordinamento, quindi è sempre sul totale dei dati,
           //nelle varie fasi di selezione e scrematura dei dati ( in caso contrario va inserito come return dello switch)

            //se filtro per tipo 
            if (!string.IsNullOrEmpty(InterventoType) && string.IsNullOrWhiteSpace(InterventoDate))
            {
                interventi = interventi.Where(s => s.TipoIntervento!.Contains(InterventoType));
            }

            //se filtro per tipo e data
            else if (!string.IsNullOrEmpty(InterventoType) && !string.IsNullOrEmpty(InterventoDate)
                && !InterventoState.HasValue && string.IsNullOrEmpty(InterventoAzienda))
            {
                interventi = interventi.Where(s => s.TipoIntervento!.Contains(InterventoType)
                && s.DataIntervento!.Contains(InterventoDate));
            }
            //se filtro per tipo e data e stato
            else if (!string.IsNullOrEmpty(InterventoType) && !string.IsNullOrEmpty(InterventoDate)
                && InterventoState.HasValue && string.IsNullOrEmpty(InterventoAzienda))
            {
                interventi = interventi.Where(s => s.TipoIntervento!.Contains(InterventoType)
                && s.DataIntervento!.Contains(InterventoDate)
                && (s.Completato == InterventoState) 
                );
            }

            //se filtro per tipo e data e stato e nome Azienda
            else if (!string.IsNullOrEmpty(InterventoType) && !string.IsNullOrEmpty(InterventoDate)
                && InterventoState.HasValue  && !string.IsNullOrEmpty(InterventoAzienda))
            {
                interventi = interventi.Where(s => s.TipoIntervento!.Contains(InterventoType)
                && s.DataIntervento!.Contains(InterventoDate)
                && (s.Completato == InterventoState)
                && s.Azienda.NomeAzienda!.Contains(InterventoAzienda)
                );
            }

            var filterList = new FiltroIntervento
            {
                //Le "selectlist" aggiornano la lista della select
                TipoIntervento = new SelectList(await tipoQuery.Distinct().ToListAsync()),

                DataIntervento = new SelectList(await dataQuery.Distinct().ToListAsync()),

                AziendaIntervento = new SelectList(await AziendaQuery.Distinct().ToListAsync()),

                // All ritorna gli elementi filtrati
                AllInterventiFiltrati = await interventi.ToListAsync()
            };

            return View(filterList);

            //var myWebSiteContext = _context.Intervento.Include(i => i.Azienda);

            // return View(await myWebSiteContext.ToListAsync());
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
        public async Task<IActionResult> Create(Intervento intervento, Azienda azienda)
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

            // qui passo il nome della azienda alla view 
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
                if (esito == true) intervento.Completato = true;
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
