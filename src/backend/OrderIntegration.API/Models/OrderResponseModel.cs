namespace OrderIntegration.API.Models
{
    /// <summary>
    /// Classe de resposta do pedido
    /// </summary>
    public class OrderResponseModel
    {
        /// <summary>
        /// Id do pedido
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cliente do pedido
        /// </summary>
        public string Cliente { get; set; } = string.Empty;

        /// <summary>
        /// Valor do pedido
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Data do pedido
        /// </summary>
        public DateTime DataPedido { get; set; }
    }
}
