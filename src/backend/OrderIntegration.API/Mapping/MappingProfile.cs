using AutoMapper;
using OrderIntegration.API.Models;
using OrderIntegration.Application.DTOs;
using System.Diagnostics.CodeAnalysis;

namespace OrderIntegration.API.Mapping
{
    /// <summary>
    /// Mapeamento de modelos
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MappingProfile: Profile
    {
        /// <summary>
        /// Perfis de mapeamento
        /// </summary>
        public MappingProfile()
        {
            CreateMap<OrderRequestModel, OrderDTO>().ReverseMap();
            CreateMap<OrderResponseModel, OrderDTO>().ReverseMap();
        }
    }
}
