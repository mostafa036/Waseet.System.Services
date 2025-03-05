using AutoMapper;
using Waseet.System.Services.Application.Dtos;
using Waseet.System.Services.Application.Resolving;
using Waseet.System.Services.Domain.Identity;
using Waseet.System.Services.Domain.Models;

namespace Waseet.System.Services.APIs.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Map Product -> ProductToReturnDto
            CreateMap<Product, ProductToReturnDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.ProductReviews.Any()
                ? src.ProductReviews.Average(r => r.Rating) : 0));


            // Map User -> UserReturnDto
            CreateMap<User, UserReturnDto>()
                .ForMember(dest => dest.ServiceProviderName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.ServiceProviderImg, opt => opt.MapFrom(src => src.profilePhotoes.FilePath));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CategoryPictureResolver>()); // FIX: Close the method properly
        }
    }
}