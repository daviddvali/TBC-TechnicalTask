using AutoMapper;
using TBC.Task.API.Models;
using TBC.Task.Domain;

namespace TBC.Task.API.AutoMapper;

internal sealed class PersonProfile : Profile
{
	public PersonProfile()
	{
		CreateMap<RequestPersonModel, Person>();
		
		CreateMap<Person, ResponsePersonModel>()
			.ForMember(dest => dest.City, m => m.MapFrom(src => src.City != null ? src.City.Name : null));
		
		CreateMap<Person, ResponsePersonWithRelatedModel>()
			.ForMember(dest => dest.City, m => m.MapFrom(src => src.City != null ? src.City.Name : null))
            .ForMember(dest => dest.RelatedTo, opt => opt.Ignore());
	}
}
