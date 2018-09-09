using System;
using AutoMapper;
using Product.Api.Contract;
using Product.Api.DomainCore.Commands;
using Product.Api.DomainCore.Models;

namespace Product.Api.LocalInfrastructure
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateProductsMaps();
        }

        public void CreateProductsMaps()
        {
            CreateMap<DomainCore.Models.Product, Contract.ViewProduct>()
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Image));

            CreateMap<DomainCore.Models.File, Contract.ImageFile>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => Convert.ToBase64String(src.Content)))
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<Contract.CreateProduct, UpdateProductCommand>()
                .ForMember(dest => dest.FileContent, opt => opt.MapFrom(src => src.Photo.Content))
                .ForMember(dest => dest.FileContentType, opt => opt.MapFrom(src => src.Photo.ContentType))
                .ForMember(dest => dest.FileTitle, opt => opt.MapFrom(src => src.Photo.Title));

            CreateMap<Contract.CreateProduct, CreateProductCommand>()
                .ForMember(dest => dest.FileContent, opt => opt.MapFrom(src => src.Photo.Content))
                .ForMember(dest => dest.FileContentType, opt => opt.MapFrom(src => src.Photo.ContentType))
                .ForMember(dest => dest.FileTitle, opt => opt.MapFrom(src => src.Photo.Title));

            CreateMap<DomainCore.Models.Product, Contract.CreateProduct>()
            .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Image));

            CreateMap<Contract.CreateProduct, DomainCore.Models.Product>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Photo));

            CreateMap<CreateProductCommand, DomainCore.Models.Product>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(x => x.Price))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(x => x.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(x => new File
                {
                    Content = Convert.FromBase64String(x.FileContent),
                    ContentType = x.FileContentType,
                    Title = x.FileTitle
                }));

//            CreateMap<CreateProductCommand, DomainCore.Models.File>()
//                .ForMember(dest => dest.Content, opt => opt.MapFrom(x => Convert.FromBase64String(x.FileContent)))
//                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(x => x.FileContentType))
//                .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.FileTitle));

            CreateMap<ImageFile, File>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(x => Convert.FromBase64String(x.Content)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore());


        }
    }
}
