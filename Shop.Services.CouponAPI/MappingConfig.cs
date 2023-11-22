using AutoMapper;
using Shop.Services.CouponAPI.Models;
using Shop.Services.CouponAPI.Models.Dto;

namespace Shop.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
