using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Product.Api.Contract;
using Product.Api.Contract.Responses;
using ProductUI.Models;

namespace ProductUI
{
    public class ProductMapProfile : Profile
    {
        public ProductMapProfile()
        {
            CreateProductsMaps();
        }

        private void CreateProductsMaps()
        {
            CreateMap<ViewProduct, ProductViewModel>()
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.Content));

            CreateMap<ProductEditModel, CreateProduct>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Image));

            CreateMap<IFormFile, ImageFile>()
                .ForMember(dest => dest.Content, opt => opt.Ignore())
                .ForMember(dest => dest.ContentType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

            CreateMap<ViewProduct, ProductEditModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CodeBeforeEdit, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.Content))
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<ViewProduct, ProductViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo.Content));
        }
    }
}
