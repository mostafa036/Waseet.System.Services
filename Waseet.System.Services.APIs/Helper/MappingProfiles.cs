using AutoMapper;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Application.Resolving;
using Waseet.System.Services.Domain.Models;
using Waseet.System.Services.Domain.Models.Identity;

namespace Waseet.System.Services.APIs.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //CreateMap<User, UserReturnDto>()
            //    .ForMember(dest => dest.ServiceProviderName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.ServiceProviderImg, opt => opt.MapFrom(src => src.profilePhotoes.FilePath));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CategoryPictureResolver>()); // FIX: Close the method properly

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.OldPrice, opt => opt.MapFrom(src => src.OldPrice))
                .ForMember(dest => dest.ServiceProviderImage, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceProviderName, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.ProductReviews.Any() ? src.ProductReviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ImageURL, opt => opt.MapFrom<ProductPictureResolver>()); // ✅ Ensure PascalCase


            CreateMap<ProductReview, ProductReviewReturnDto>()
                .ForMember(dest => dest.comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.date, opt => opt.MapFrom(src => src.ReviewDate))
                .ForMember(dest => dest.name, opt => opt.Ignore()) // Assuming email as fallback
                .ForMember(dest => dest.profileImage, opt => opt.Ignore()); // Will set separately


            CreateMap<User, ProductReviewReturnUserData>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.CustomerImage, opt => opt.MapFrom<ProductReviewResolver>());

            CreateMap<Product, ProductCards>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.OldPrice, opt => opt.MapFrom(src => src.OldPrice))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.ProductReviews.Any() ? src.ProductReviews.Average(r => r.Rating) : 0))
                .ForMember(dest => dest.ImageURL, opt => opt.Ignore()) // ✅ Ensure PascalCase
                .ForMember(dest => dest.ServiceProviderImage, opt => opt.Ignore())
                .ForMember(dest => dest.ServiceProviderName, opt => opt.Ignore()); // We'll fetch this separately

        }
    }
}