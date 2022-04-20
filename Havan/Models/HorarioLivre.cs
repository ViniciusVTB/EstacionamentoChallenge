using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Havan.Models
{
    public class HorarioLivre
    {
        [Key()]
        public int Id { get; set; }
        [DisplayName("Dia")]
        public string Dia { get; set; }
        [DisplayName("Hora inicial")]
        public DateTime HoraInicial { get; set; }
        [DisplayName("Hora final")]
        public DateTime HoraFinal { get; set; }
    }
}
