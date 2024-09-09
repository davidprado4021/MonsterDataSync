using AutoMapper;
using EFPractica01.DTOs;
using EFPractica01.Models;
using MonsterDataSync.DTOs;
using System.Runtime;

namespace MonsterDataSync.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<SkillsDTOs, Skills>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Skills, SkillsDTOs>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Skills, Skills>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<Ranks, Ranks>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            // Evitar ciclos al ignorar las propiedades de Ranks
            CreateMap<ViewSkillsDtos, Skills>()
                .ForMember(dest => dest.Ranks, opt => opt.Ignore());
            CreateMap<RanksDTOs, Ranks>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());


            // Mapeo de Skills a ViewSkillsDtos, incluyendo la lista de RanksDTOs
            CreateMap<Skills, ViewSkillsDtos>()
                .ForMember(dest => dest.Ranks, opt => opt.MapFrom(src => src.Ranks)); ;
            // Mapeo de Ranks a RanksDTOs
            CreateMap<Ranks, RanksDTOs>();
            CreateMap<RanksUPDTOs, Ranks>()
              .ForMember(dest => dest.Skills, opt => opt.Ignore());


        }
    }
}
