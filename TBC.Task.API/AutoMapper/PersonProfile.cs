using AutoMapper;
using TBC.Task.API.Models;
using TBC.Task.Domain;
using TBC.Task.Domain.ComplexTypes;

namespace TBC.Task.API.AutoMapper;

internal class PersonProfile : Profile
{
    public PersonProfile() => CreateMap<PersonModel, Person>()
        .ForMember(dest => dest.ContactInfo, act => act.MapFrom(src => new ContactInfo()
        {
            MobilePhone = src.MobilePhone,
            HomePhone = src.HomePhone,
            WorkPhone = src.WorkPhone
        }));
}