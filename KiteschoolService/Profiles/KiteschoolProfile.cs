using AutoMapper;
using KiteschoolService.Dtos;
using KiteschoolService.Models;
using UserService;

namespace KiteschoolService.Profiles
{
    public class KiteschoolProfile : Profile
    {
        public KiteschoolProfile()
        {
            // Source -> Target
            CreateMap<Kiteschool, KiteschoolReadDto>();
            // CreateMap<CommandCreateDto, Command>();
            // CreateMap<Command, CommandReadDto>();
            CreateMap<KiteschoolPublishedDto, Kiteschool>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcKiteschoolModel, Kiteschool>()
                // this line is necessary.
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.KiteschoolId))
                // These lines are not necessary. (used just as a precaution)
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
            // .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}