using AutoMapper;
using OrderIntegration.Application.DTOs;
using OrderIntegration.Domain.Entities;

namespace OrderIntegration.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }
}
