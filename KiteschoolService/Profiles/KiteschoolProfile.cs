using AutoMapper;
using KiteschoolService.Dtos;
using KiteschoolService.Models;

namespace KiteschoolService.Profiles
{
    public class KiteschoolProfile : Profile
    {
        public KiteschoolProfile()
        {
            // Source -> Target
            CreateMap<Kiteschool, KiteschoolReadDto>();
            CreateMap<KiteschoolCreateDto, Kiteschool>();
            CreateMap<KiteschoolPublishedDto, Kiteschool>();
        }
    }
}