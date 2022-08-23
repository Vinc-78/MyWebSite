using Microsoft.AspNetCore.Mvc.Rendering;


namespace MyWebSite.Models
{
    public class FiltroIntervento
    {
        public List<Intervento>? AllInterventiFiltrati { get; set; }
        public SelectList? TipoIntervento { get; set; } // creo la lista per il select

        public SelectList? DataIntervento { get; set; }

        public SelectList? StatoIntervento { get; set; }

        public SelectList? AziendaIntervento { get; set; }

        public string? InterventoType { get; set; } // passo la stringa per filtrare

        public string? InterventoDate { get; set; }

        public string? InterventoState { get; set; }

        public string? InterventoAzienda { get; set; }
    }
}
