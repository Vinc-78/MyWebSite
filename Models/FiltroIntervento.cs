using Microsoft.AspNetCore.Mvc.Rendering;


namespace MyWebSite.Models
{
    public class FiltroIntervento
    {
        public List<Intervento>? Interventos { get; set; }
        public SelectList? TipoIntervento { get; set; }

        public string? InterventoType { get; set; }
    }
}
