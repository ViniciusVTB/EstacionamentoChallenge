using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Havan.Models
{
    public class Preco
    {     
        [Key()]
        public int Id { get; set; }
        [DisplayName("Preço hora")]
        public int PrecoHora { get; set; }
        [DisplayName("Preço adicional")]
        public int PrecoHoraAdicional { get; set; }
        [DisplayName("Data vigente Inicial")]
        public DateTime DataInicial { get; set; }
        [DisplayName("Data vigente final")]
        public DateTime DataFinal { get; set; }
    }

}
