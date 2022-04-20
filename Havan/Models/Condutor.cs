using System.ComponentModel.DataAnnotations;

namespace Havan.Models
{
    public class Condutor
    {
        [Key()]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int? Desconto { get; set; }
    }
}
