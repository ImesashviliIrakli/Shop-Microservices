using AutoMapper;
using Shop.Services.ShoppingCartAPI.Models;
using Shop.Services.ShoppingCartAPI.Models.Dto;

namespace Shop.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
