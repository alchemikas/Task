using System;
using AutoMapper;

namespace Product.Api.Infrastructure
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateProductsMaps();
        }

        public void CreateProductsMaps()
        {
            CreateMap<Models.Product, Contract.Models.Product>()
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Image));


            CreateMap<Models.File, Contract.Models.ImageFile>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.Title))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(x => Convert.ToBase64String(x.Content)))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(x => x.ContentType));
        }
    }
}
