using AutoMapper;
using SchoolRowingApp.Domain.Entities;
using SchoolRowingApp.Application.Athletes.Dto;

namespace SchoolRowingApp.Application.Common.Mappings;

public class AthleteMappingProfile : Profile
{
    public AthleteMappingProfile()
    {
        CreateMap<Athlete, AthleteDto>();
    }
}