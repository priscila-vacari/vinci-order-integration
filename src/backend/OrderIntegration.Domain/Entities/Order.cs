using System.ComponentModel.DataAnnotations.Schema;

namespace OrderIntegration.Domain.Entities
{
    [Table("orders")]
    public class Order
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
    }
}
