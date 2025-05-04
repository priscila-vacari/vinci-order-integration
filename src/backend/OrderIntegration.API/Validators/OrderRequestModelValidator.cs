using FluentValidation;
using OrderIntegration.API.Models;

namespace OrderIntegration.API.Validators
{
    /// <summary>
    /// Classe de validação de lançamento
    /// </summary>
    public class OrderRequestModelValidator : AbstractValidator<OrderRequestModel>
    {
        /// <summary>
        /// Validador de lançamento
        /// </summary>
        public OrderRequestModelValidator()
        {
            RuleFor(x => x.DataPedido)
                .NotEmpty().WithMessage("A data do pedido é obrigatória.")
                .GreaterThanOrEqualTo(DateTime.Now.AddMonths(-12)).WithMessage("A data do pedido deve ser de 1 ano atrás para frente.")
                .LessThan(DateTime.Now).WithMessage("A data deve ser no máximo igual a hoje.");

            RuleFor(x => x.Cliente)
                .NotEmpty().WithMessage("O cliente é obrigatório.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("A quantia deve ser maior que zero.") 
                .LessThan(100000).WithMessage("A quantia deve ser menor que 100.000.")
                .PrecisionScale(10, 2, true).WithMessage("A quantia deve ter no máximo 10 dígitos e 2 casas decimais."); 
        }
    }
}
