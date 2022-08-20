using System.ComponentModel.DataAnnotations;

namespace MyWebSite.Models
{
    public class Azienda
    {
        [Key]
        public int AziendaId { get; set; }

        public string NomeAzienda { get; set; } = null!;

        public string Settore { get; set; } = null!;

        public string? Città { get; set; }

        public string? Indirizzo { get; set; }
    }
}
