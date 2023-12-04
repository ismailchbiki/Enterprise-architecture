using AutoMapper;
using UserService.Dtos;

namespace UserService.Profiles
{
    public class KiteschoolsProfile : Profile
    {
        public KiteschoolsProfile()
        {
            CreateMap<KiteschoolCreateDto, KiteschoolPublishedDto>();
        }
    }
}