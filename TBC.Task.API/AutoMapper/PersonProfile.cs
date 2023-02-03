using AutoMapper;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Domain.ComplexTypes;

namespace TBC.Task.API.AutoMapper;

internal class PersonProfile : Profile
{
	public PersonProfile()
	{
		CreateMap<RequestPersonModel, Person>()
			.ForMember(dest => dest.ContactInfo, act => act.MapFrom(src => new ContactInfo
			{
				MobilePhone = src.MobilePhone,
				HomePhone = src.HomePhone,
				WorkPhone = src.WorkPhone
			}));

		CreateMap<Person, ResponsePersonModel>()
			.ForMember(dest => dest.MobilePhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone))
			.ForMember(dest => dest.HomePhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone))
			.ForMember(dest => dest.WorkPhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone));
		
		CreateMap<Person, ResponsePersonWithRelatedModel>()
			.ForMember(dest => dest.MobilePhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone))
			.ForMember(dest => dest.HomePhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone))
			.ForMember(dest => dest.WorkPhone, act => act.MapFrom(src => src.ContactInfo!.MobilePhone));
	}
}
