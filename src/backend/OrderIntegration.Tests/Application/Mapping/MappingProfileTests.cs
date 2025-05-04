using AutoMapper;
using OrderIntegration.Application.DTOs;
using OrderIntegration.Application.Mapping;
using OrderIntegration.Domain.Entities;

namespace OrderIntegration.Tests.Application.Mapping
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

            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_OrderDTO_To_Order()
        {
            var dto = new OrderDTO
            {
                Id = 1,
                Cliente = "João",
                Valor = 150.75m,
                DataPedido = DateTime.Today
            };

            var entity = _mapper.Map<Order>(dto);

            Assert.Equal(dto.Id, entity.Id);
            Assert.Equal(dto.Cliente, entity.Cliente);
            Assert.Equal(dto.Valor, entity.Valor);
            Assert.Equal(dto.DataPedido, entity.DataPedido);
        }

        [Fact]
        public void Should_Map_Order_To_OrderDTO()
        {
            var entity = new Order
            {
                Id = 2,
                Cliente = "Maria",
                Valor = 220.00m,
                DataPedido = DateTime.Today.AddDays(-2)
            };

            var dto = _mapper.Map<OrderDTO>(entity);

            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.Cliente, dto.Cliente);
            Assert.Equal(entity.Valor, dto.Valor);
            Assert.Equal(entity.DataPedido, dto.DataPedido);
        }
    }
}
