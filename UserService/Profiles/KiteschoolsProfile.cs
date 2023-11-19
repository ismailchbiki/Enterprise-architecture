using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Profiles
{
    public class KiteschoolsProfile : Profile
    {
        public KiteschoolsProfile()
        {
            // Source -> Target
            CreateMap<Kiteschool, KiteschoolReadDto>();
            CreateMap<KiteschoolCreateDto, Kiteschool>();
            CreateMap<KiteschoolReadDto, KiteschoolPublishedDto>();
            CreateMap<Kiteschool, GrpcKiteschoolModel>()
                .ForMember(dest => dest.KiteschoolId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}