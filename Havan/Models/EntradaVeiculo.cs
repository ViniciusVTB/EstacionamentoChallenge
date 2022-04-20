using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Havan.Models
{
    public class EntradaVeiculo
    {
        [Key()]
        public int Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public bool Status { get; set; }

        [DisplayName("Data de entrada")]
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public string? Permanencia { get; set; }
        public int? ValorHora { get; set; }
        public int? ValorHoraAdicional { get; set; }
        public double? Total { get; set; }

        [DisplayName("Condutor")]
        public int? CondutorId { get; set; }
        public virtual Condutor? Condutor { get; set; }
    }
}
