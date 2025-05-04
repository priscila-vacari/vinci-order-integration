using AutoMapper;
using OrderIntegration.API.Mapping;
using OrderIntegration.API.Models;
using OrderIntegration.Application.DTOs;

namespace OrderIntegration.Tests.Mapping
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            config.AssertConfigurationIsValid(); 

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_OrderRequestModel_To_OrderDTO()
        {
            var model = new OrderRequestModel
            {
                Id = 1,
                Cliente = "Cliente A",
                Valor = 100,
                DataPedido = DateTime.Today
            };

            var dto = _mapper.Map<OrderDTO>(model);

            Assert.Equal(model.Id, dto.Id);
            Assert.Equal(model.Cliente, dto.Cliente);
            Assert.Equal(model.Valor, dto.Valor);
            Assert.Equal(model.DataPedido, dto.DataPedido);
        }

        [Fact]
        public void Should_Map_OrderDTO_To_OrderResponseModel()
        {
            var dto = new OrderDTO
            {
                Id = 1,
                Cliente = "Cliente A",
                Valor = 100,
                DataPedido = DateTime.Today
            };

            var model = _mapper.Map<OrderResponseModel>(dto);

            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Cliente, model.Cliente);
            Assert.Equal(dto.Valor, model.Valor);
            Assert.Equal(dto.DataPedido, model.DataPedido);
        }
    }
}
