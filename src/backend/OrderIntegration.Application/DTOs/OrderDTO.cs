namespace OrderIntegration.Application.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
    }
}
