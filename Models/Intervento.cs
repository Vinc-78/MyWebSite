using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebSite.Models
{
    public class Intervento
    {
        [Key]
        public int InterventoId { get; set; }

        public string TipoIntervento { get; set; } = null!;
        public string DataIntervento { get; set; } = null!;

        public bool? Completato { get; set; }

        [ForeignKey("Azienda")]
        public int AziendaID { get; set; }

        public virtual Azienda Azienda { get; private set; }= null!;
    }
}
