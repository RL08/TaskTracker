using AutoMapper;
using TaskTrackerProject.Application.Dto;
using TaskTrackerProject.Application.Model;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();  // UserDto --> User
        CreateMap<User, UserDto>();  // User --> UserDto
    }
}