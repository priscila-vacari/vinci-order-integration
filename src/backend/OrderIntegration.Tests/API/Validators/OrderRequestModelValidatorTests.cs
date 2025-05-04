using FluentValidation.TestHelper;
using OrderIntegration.API.Models;
using OrderIntegration.API.Validators;

namespace OrderIntegration.Tests.Validators
{
    public class OrderRequestModelValidatorTests
    {
        private readonly OrderRequestModelValidator _validator;

        public OrderRequestModelValidatorTests()
        {
            _validator = new OrderRequestModelValidator();
        }

        [Fact]
        public void Should_Have_Error_When_DataPedido_Is_Empty()
        {
            var model = new OrderRequestModel { DataPedido = default };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DataPedido)
                .WithErrorMessage("A data do pedido é obrigatória.");
        }

        [Fact]
        public void Should_Have_Error_When_DataPedido_Too_Old()
        {
            var model = new OrderRequestModel { DataPedido = DateTime.Now.AddYears(-2) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DataPedido);
        }

        [Fact]
        public void Should_Have_Error_When_DataPedido_In_Future()
        {
            var model = new OrderRequestModel { DataPedido = DateTime.Now.AddDays(1) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.DataPedido);
        }

        [Fact]
        public void Should_Have_Error_When_Cliente_Is_Empty()
        {
            var model = new OrderRequestModel { Cliente = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Cliente);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Have_Error_When_Valor_Is_Invalid(decimal valor)
        {
            var model = new OrderRequestModel { Valor = valor };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Should_Have_Error_When_Valor_Too_High()
        {
            var model = new OrderRequestModel { Valor = 100000 };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Valor);
        }

        [Fact]
        public void Should_Pass_When_Valid_Model()
        {
            var model = new OrderRequestModel
            {
                DataPedido = DateTime.Now.AddDays(-1),
                Cliente = "Cliente Exemplo",
                Valor = 999.99m
            };

            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
