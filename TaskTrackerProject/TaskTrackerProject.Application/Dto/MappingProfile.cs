using AutoMapper;
using System.Threading.Tasks;
using TaskTrackerProject.Application.Model;

namespace TaskTrackerProject.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserListDto, UserList>();  // ListDto --> List
            CreateMap<UserList, UserListDto>();  // List --> ListDto
            CreateMap<ListTaskDto, ListTask>();  // ListTaskDto --> ListTask
            CreateMap<ListTask, ListTaskDto>();  // ListTask --> ListTaskDto
        }
    }
}