//using AutoMapper;
//using SchoolRowingApp.Domain.Entities;
//using SchoolRowingApp.Application.Athletes.Dto;

//namespace SchoolRowingApp.Application.Common.Mappings;

////public class AthleteMappingProfile : Profile
////{
////    public AthleteMappingProfile()
////    {
////        CreateMap<Athlete, AthleteDto>();
////    }
////}

//// Маппинг в Application Mapper
//public class AthleteProfile : Profile
//{
//    public AthleteProfile()
//    {
//        CreateMap<Athlete, AthleteDto>()
//            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
//            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.SecondName))
//            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
//            .ForMember(dest => dest.Payers, opt => opt.MapFrom(src =>
//                src.AthletePayers.Select(ap => new AthletePayerDto(
//                    ap.PayerId,ap.
               
//                ))));
//    }

//    private string GetPayerTypeDescription(PayerType type)
//    {
//        return type switch
//        {
//            PayerType.Self => "Сам атлет",
//            PayerType.Mother => "Мама",
//            PayerType.Father => "Папа",
//            PayerType.Uncle => "Дядя",
//            PayerType.Other => "Другое",
//            _ => "Неизвестно"
//        };
//    }
//}