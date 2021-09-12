using AutoMapper;
using DrugsManager.Models;

namespace DrugsManager.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Drug, DrugDto>();
            CreateMap<DrugDto, Drug>();
        }
    }
}
