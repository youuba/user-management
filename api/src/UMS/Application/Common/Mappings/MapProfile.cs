using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using UMS.Application.Common.Models;

namespace UMS.Application.Common.Mappings;

[ExcludeFromCodeCoverage]
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
