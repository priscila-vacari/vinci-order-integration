namespace OrderIntegration.API.Models
{
    /// <summary>
    /// Classe de requisição do pedido
    /// </summary>
    public class OrderRequestModel
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
